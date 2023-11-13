using FluentResults;
using InteractiveScheduleUad.Api.Models;
using InteractiveScheduleUad.Api.Models.Dtos;
using System.Security.Claims;

namespace InteractiveScheduleUad.Api.Services.AuthAndAuthorizationServices.Contracts;

public interface IAuthService
{
    Task<Result<UserForReadDto>> Register(UserForRegisterDto userForRegisterDto);

    Task<Result> DeleteAsync(string username);

    Task<Result<IEnumerable<UserForReadDto>>> GetAllAsync();

    Task<Result> UpdateAsync(string username, UserForRegisterDto userForRegisterDto);

    Task<Result<AuthenticatedResponse>> Login(UserForLoginDto userForLoginDto);

    Task<Result> Logout(ClaimsPrincipal claims);

    Task<Result<string>> RefreshToken(ClaimsPrincipal claims);
}