using InteractiveScheduleUad.Api.Models;
using InteractiveScheduleUad.Api.Models.Dtos;
using System.Security.Claims;

namespace InteractiveScheduleUad.Api.Services.Contracts;

public interface IAuthService
{
    Task<Result<UserForReadDto>> Register(UserForRegisterDto userForRegisterDto);

    Task<bool> DeleteAsync(string username);

    Task<IEnumerable<UserForReadDto>> GetAllAsync();

    Task<bool> UpdateAsync(string username, UserForRegisterDto userForRegisterDto);

    Task<Result<AuthenticatedResponse>> Login(UserForLoginDto userForLoginDto);

    Task<Result<bool>> Logout(ClaimsPrincipal claims);

    Task<Result<AuthenticatedResponse>> RefreshToken(ClaimsPrincipal claims);
}