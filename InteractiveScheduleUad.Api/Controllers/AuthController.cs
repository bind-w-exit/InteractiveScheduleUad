﻿using InteractiveScheduleUad.Api.Extensions;
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

    [HttpPost("Register")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(UserForReadDto), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<UserForReadDto>> Register(UserForRegisterDto userForRegisterDto)
    {
        var result = await _authService.Register(userForRegisterDto);

        if (result.IsFailed)
            return result.Errors.First().ToObjectResult();

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

        if (result.IsFailed)
            return result.Errors.First().ToObjectResult();

        return Ok(result.Value);
    }

    [HttpDelete("Logout")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult> Logout()
    {
        var result = await _authService.Logout(User);

        if (result.IsFailed)
            return result.Errors.First().ToObjectResult();

        return Ok();
    }

    [HttpGet("RefreshToken")]
    [Authorize(Roles = "RefreshToken")]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<string>> RefreshToken()
    {
        var result = await _authService.RefreshToken(User);

        if (result.IsFailed)
            return result.Errors.First().ToObjectResult();

        return Ok(result.Value);
    }

    // TODO: Add DELETE, GET all, and PUT methods
}