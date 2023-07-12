﻿using InteractiveScheduleUad.Api.Models;

namespace InteractiveScheduleUad.Api.Services.Contracts;

public interface ITokenService
{
    AuthenticatedResponse GenerateAuthenticatedResponse(User user);

    AuthenticatedResponse GenerateAuthenticatedResponse(User user,
        out Guid pairJti,
        out DateTime accessTokenExpires,
        out DateTime refreshTokenExpires);

    string GenerateRefreshToken(User user, Guid pairJti);
}