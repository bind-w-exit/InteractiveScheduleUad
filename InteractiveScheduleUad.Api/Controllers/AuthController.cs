using FluentResults;
using InteractiveScheduleUad.Api.Extensions;
using InteractiveScheduleUad.Api.Models;
using InteractiveScheduleUad.Api.Models.Dtos;
using InteractiveScheduleUad.Api.Services.AuthAndAuthorizationServices.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace InteractiveScheduleUad.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    // only admin can create new users
    [HttpPost("Register")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(UserForReadDto), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<UserForReadDto>> Register(UserForRegisterDto userForRegisterDto)
    {
        var result = await _authService.Register(userForRegisterDto);
        return HandleResult(result);
    }

    [HttpPost("Login")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(AuthenticatedResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public async Task<ActionResult<AuthenticatedResponse>> Login(UserForLoginDto userForLoginDto)
    {
        var result = await _authService.Login(userForLoginDto);
        return HandleResult(result);
    }

    [HttpDelete("Logout")]
    [Authorize(Roles = nameof(UserRole.Admin))]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult> Logout()
    {
        var result = await _authService.Logout(User);
        return HandleResult(result);
    }

    [HttpPost("RefreshToken")]
    //[Authorize(Roles = "RefreshToken")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<string>> RefreshToken([FromBody] string refreshToken)
    {
        var result = await _authService.RefreshToken(refreshToken);
        return HandleResult(result);
    }

    // utils

    private ActionResult HandleResult<T>(Result<T> result)
    {
        if (result.IsFailed)
        {
            var objectResult = result.Errors.First().ToObjectResult();
            return objectResult;
        }

        return Ok(result.Value);
    }

    private ActionResult HandleResult(Result result)
    {
        return HandleResult<object>(result);
    }

    // TODO: Add DELETE, GET all, and PUT methods
}