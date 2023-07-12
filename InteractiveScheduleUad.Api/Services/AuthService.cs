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
            return new InvalidOperationException("Username already exist");

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
            return new KeyNotFoundException("User not found");

        if (!_hashService.VerifyPasswordHash(userForLoginDto.Password, userFromDb.PasswordHash, userFromDb.PasswordSalt))
            return new InvalidOperationException("Wrong password");

        AuthenticatedResponse response = _tokenService.GenerateAuthenticatedResponse(userFromDb);

        return response;
    }

    // TODO: Change validation checks order
    public async Task<Result<bool>> Logout(ClaimsPrincipal claims)
    {
        // Validation 1: Ensure that the JTI is valid
        var jtiString = claims.FindFirstValue(JwtRegisteredClaimNames.Jti);
        if (string.IsNullOrEmpty(jtiString) || !Guid.TryParse(jtiString, out var jti))
        {
            return new FormatException("Invalid JTI");
        }

        // Validation 2: Check if the token is already in the blacklist
        var revokedToken = await _revokedTokenRepository.SingleOrDefaultAsync(t => t.Jti == jti);
        if (revokedToken is not null)
        {
            return true;
        }

        // Validation 3: Ensure that the token expiry time is valid
        var tokenExpiresString = claims.FindFirstValue(JwtRegisteredClaimNames.Exp);
        if (string.IsNullOrEmpty(tokenExpiresString) || !long.TryParse(tokenExpiresString, out long tokenExpiresSeconds))
        {
            return new FormatException("Invalid Exp");
        }

        var tokenExpires = DateTimeOffset.FromUnixTimeSeconds(tokenExpiresSeconds);
        if (tokenExpires <= DateTimeOffset.UtcNow)
        {
            return new InvalidOperationException("Token has expired");
        }

        // Add tokens to the blacklist
        await _revokedTokenRepository.InsertAsync(new RevokedToken
        {
            Jti = jti,
            ExpiryDate = tokenExpires.UtcDateTime
        });
        await _revokedTokenRepository.SaveChangesAsync();

        return true;
    }

    // TODO: Check functionality
    public async Task<Result<string>> RefreshToken(ClaimsPrincipal claims)
    {
        // Validation 1: Ensure that the JTI is valid
        var jtiString = claims.FindFirstValue(JwtRegisteredClaimNames.Jti);
        if (string.IsNullOrEmpty(jtiString) || !Guid.TryParse(jtiString, out var jti))
        {
            return new FormatException("Invalid JTI");
        }

        // TODO: Move to the AuthMiddleware
        // Validation 2: Ensure that the token is not in the blacklist
        var revokedToken = await _revokedTokenRepository.SingleOrDefaultAsync(t => t.Jti == jti);
        if (revokedToken != null)
        {
            return new InvalidOperationException("Token has been used");
        }

        // Validation 3: Ensure that the token expiry time is valid
        var tokenExpiresString = claims.FindFirstValue(JwtRegisteredClaimNames.Exp);
        if (string.IsNullOrEmpty(tokenExpiresString) || !long.TryParse(tokenExpiresString, out long tokenExpiresSeconds))
        {
            return new FormatException("Invalid Exp");
        }
        var tokenExpires = DateTimeOffset.FromUnixTimeSeconds(tokenExpiresSeconds);

        // Validation 4: Ensure that the user ID is valid
        var userId = claims.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out int userIdValue))
        {
            return new FormatException("Invalid user ID");
        }

        var user = await _userRepository.GetByIdAsync(userIdValue);
        if (user == null)
        {
            return new KeyNotFoundException("User not found");
        }

        return _tokenService.GenerateRefreshToken(user, jti);
    }
}