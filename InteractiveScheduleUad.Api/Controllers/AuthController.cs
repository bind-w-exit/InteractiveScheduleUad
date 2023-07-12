using InteractiveScheduleUad.Api.Models;
using InteractiveScheduleUad.Api.Models.Dtos;
using InteractiveScheduleUad.Api.Services.Contracts;
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

    [HttpPost("Register")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(UserForReadDto), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<UserForReadDto>> Register(UserForRegisterDto userForRegisterDto)
    {
        var result = await _authService.Register(userForRegisterDto);

        if (!result.IsSuccess)
        {
            return result.Exception switch
            {
                InvalidOperationException => BadRequest(result.Exception.Message),
                _ => BadRequest(result.Exception),
            };
        }

        return Ok(result.Value);
    }

    [HttpPost("Login")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(AuthenticatedResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public async Task<ActionResult<AuthenticatedResponse>> Login(UserForLoginDto userForLoginDto)
    {
        var result = await _authService.Login(userForLoginDto);

        if (!result.IsSuccess)
        {
            return result.Exception switch
            {
                KeyNotFoundException => NotFound(result.Exception.Message),
                InvalidOperationException => BadRequest(result.Exception.Message),
                _ => BadRequest(result.Exception),
            };
        }
        else
            return Ok(result.Value);
    }

    [HttpDelete("Logout")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult> Logout()
    {
        var result = await _authService.Logout(User);

        if (!result.IsSuccess)
        {
            return result.Exception switch
            {
                FormatException => BadRequest(result.Exception.Message),
                InvalidOperationException => BadRequest(result.Exception.Message),
                _ => BadRequest(result.Exception),
            };
        }
        else
            return Ok();
    }

    [HttpGet("RefreshToken")]
    [Authorize(Roles = "RefreshToken")]
    [ProducesResponseType(typeof(AuthenticatedResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<AuthenticatedResponse>> RefreshToken()
    {
        var result = await _authService.RefreshToken(User);

        if (!result.IsSuccess)
        {
            return result.Exception switch
            {
                KeyNotFoundException => NotFound(result.Exception.Message),
                FormatException => BadRequest(result.Exception.Message),
                InvalidOperationException => BadRequest(result.Exception.Message),
                _ => BadRequest(result.Exception),
            };
        }
        else
            return Ok(result.Value);
    }

    // TODO: Add DELETE, GET all, and PUT methods
}