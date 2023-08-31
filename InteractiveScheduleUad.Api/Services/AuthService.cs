using FluentResults;
using InteractiveScheduleUad.Api.Errors;
using InteractiveScheduleUad.Api.Mappers;
using InteractiveScheduleUad.Api.Models;
using InteractiveScheduleUad.Api.Models.Dtos;
using InteractiveScheduleUad.Api.Repositories.Contracts;
using InteractiveScheduleUad.Api.Services.Contracts;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace InteractiveScheduleUad.Api.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IHashService _hashService;
    private readonly ITokenService _tokenService;
    private readonly IRevokedTokenRepository _revokedTokenRepository;

    public AuthService(IUserRepository userRepository, IHashService hashService, ITokenService tokenService, IRevokedTokenRepository revokedTokenRepository)
    {
        _userRepository = userRepository;
        _hashService = hashService;
        _tokenService = tokenService;
        _revokedTokenRepository = revokedTokenRepository;
    }

    public async Task<Result<UserForReadDto>> Register(UserForRegisterDto userForRegisterDto)
    {
        var userFromDb = await _userRepository.SingleOrDefaultAsync(u => u.Username == userForRegisterDto.Username);

        if (userFromDb is not null)
            return new EntityAlreadyExistsError(nameof(User.Username));

        _hashService.CreatePasswordHash(userForRegisterDto.Password, out byte[] passwordHash, out byte[] passwordSalt);

        User user = new()
        {
            Username = userForRegisterDto.Username,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt,
            UserRole = userForRegisterDto.UserRole
        };

        await _userRepository.InsertAsync(user);
        await _userRepository.SaveChangesAsync();

        return UserMapper.UserToUserForReadDto(user);
    }

    public async Task<bool> DeleteAsync(string username)
    {
        var user = await _userRepository.SingleOrDefaultAsync(u => u.Username == username);

        if (user is not null)
        {
            _userRepository.Delete(user);
            await _userRepository.SaveChangesAsync();
            return true;
        }
        return false;
    }

    public async Task<IEnumerable<UserForReadDto>> GetAllAsync()
    {
        var users = await _userRepository.GetAllAsync(true);

        List<UserForReadDto> usersForReadDto = new();
        foreach (var user in users)
        {
            usersForReadDto.Add(UserMapper.UserToUserForReadDto(user));
        }

        return usersForReadDto;
    }

    public async Task<bool> UpdateAsync(string username, UserForRegisterDto userForRegisterDto)
    {
        var userFromDb = await _userRepository.SingleOrDefaultAsync(u => u.Username == username);
        if (userFromDb is not null)
        {
            _hashService.CreatePasswordHash(userForRegisterDto.Password, out byte[] passwordHash, out byte[] passwordSalt);

            userFromDb.Username = userForRegisterDto.Username;
            userFromDb.PasswordHash = passwordHash;
            userFromDb.PasswordSalt = passwordSalt;
            userFromDb.UserRole = userForRegisterDto.UserRole;

            _userRepository.Update(userFromDb);
            await _userRepository.SaveChangesAsync();

            return true;
        }
        return false;
    }

    public async Task<Result<AuthenticatedResponse>> Login(UserForLoginDto userForLoginDto)
    {
        var userFromDb = await _userRepository.SingleOrDefaultAsync(u => u.Username == userForLoginDto.UserName);

        if (userFromDb is null)
            return new NotFoundError(nameof(User));

        if (!_hashService.VerifyPasswordHash(userForLoginDto.Password, userFromDb.PasswordHash, userFromDb.PasswordSalt))
            return new UnauthorizedError("Wrong password");

        AuthenticatedResponse response = _tokenService.GenerateAuthenticatedResponse(userFromDb);

        return response;
    }

    public async Task<Result<bool>> Logout(ClaimsPrincipal claims)
    {
        var jtiString = claims.FindFirstValue(JwtRegisteredClaimNames.Jti);
        if (string.IsNullOrEmpty(jtiString) || Guid.TryParse(jtiString, out var jti))
        {
            return new UnauthorizedError("The JTI is not a valid GUID.");
        }

        var tokenExpiresString = claims.FindFirstValue(JwtRegisteredClaimNames.Exp);
        if (string.IsNullOrEmpty(tokenExpiresString) || long.TryParse(tokenExpiresString, out long tokenExpiresSeconds))
        {
            return new UnauthorizedError("The token expiry time is not valid.");
        }
        var tokenExpires = DateTimeOffset.FromUnixTimeSeconds(tokenExpiresSeconds);

        var revokedToken = await _revokedTokenRepository.SingleOrDefaultAsync(t => t.Jti == jti);
        if (revokedToken is not null)
        {
            return true;
        }

        // Add refresh and all its access tokens to the blacklist.
        // This is done to prevent the user from using the token to access protected resources after they have logged out.
        await _revokedTokenRepository.InsertAsync(new RevokedToken
        {
            Jti = jti,
            ExpiryDate = tokenExpires.UtcDateTime
        });
        await _revokedTokenRepository.SaveChangesAsync();

        return true;
    }

    public async Task<Result<string>> RefreshToken(ClaimsPrincipal claims)
    {
        var jtiString = claims.FindFirstValue(JwtRegisteredClaimNames.Jti);
        if (string.IsNullOrEmpty(jtiString) || Guid.TryParse(jtiString, out Guid jti))
        {
            return new UnauthorizedError("The JTI is not a valid GUID.");
        }

        var userId = claims.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId) || int.TryParse(userId, out int userIdValue))
        {
            return new UnauthorizedError("The user ID is not a valid integer.");
        }

        var revokedToken = await _revokedTokenRepository.SingleOrDefaultAsync(t => t.Jti == jti);
        if (revokedToken is not null)
        {
            return new UnauthorizedError("The token has been revoked.");
        }

        var user = await _userRepository.GetByIdAsync(userIdValue);
        if (user is null)
        {
            return new UnauthorizedError("The user was not found.");
        }

        return _tokenService.GenerateRefreshToken(user, jti);
    }
}