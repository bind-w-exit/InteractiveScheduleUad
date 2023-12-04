using InteractiveScheduleUad.Api.Models;
using InteractiveScheduleUad.Api.Services.AuthAndAuthorizationServices.Contracts;
using Microsoft.IdentityModel.Tokens;
using Npgsql.Internal.TypeHandlers;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace InteractiveScheduleUad.Api.Services.AuthAndAuthorizationServices;

public class TokenService : ITokenService
{
    private readonly string _audience;
    private readonly string _issuer;
    private readonly string _key;

    private readonly int _accessTokenLifetimeMins = 5;
    private readonly int _refreshTokenLifetimeDays = 14;

    private InteractiveScheduleUadApiDbContext _dbContext;

    public TokenService(IConfiguration configuration, InteractiveScheduleUadApiDbContext dbContext)
    {
        _issuer = ValidateConfigurationProp(configuration["JWT_ISSUER"], "JWT_ISSUER");
        _audience = ValidateConfigurationProp(configuration["JWT_AUDIENCE"], "JWT_AUDIENCE");
        _key = ValidateConfigurationProp(configuration["JWT_ACCESS_TOKEN_SECRET"], "JWT_ACCESS_TOKEN_SECRET");
        _dbContext = dbContext;
    }

    private static string ValidateConfigurationProp(string? value, string parameterName)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException($"Value for parameter '{parameterName}' in the configuration file is null or whitespace.");
        }

        return value;
    }

    // TODO: Annotate
    // overload
    public AuthenticatedResponse GenerateAuthenticatedResponse(User user)
    {
        return GenerateAuthenticatedResponse(user, out _, out _, out _);
    }

    public AuthenticatedResponse GenerateAuthenticatedResponse(
        User user,
        out Guid pairJti,
        out DateTime accessTokenExpires,
        out DateTime refreshTokenExpires)
    {
        // jti = json token id
        pairJti = Guid.NewGuid();
        accessTokenExpires = DateTime.UtcNow.AddMinutes(_accessTokenLifetimeMins);
        refreshTokenExpires = DateTime.UtcNow.AddDays(_refreshTokenLifetimeDays);

        var refreshTokenClaims = GenerateRefreshTokenClaims(user, pairJti);

        return new AuthenticatedResponse
        {
            AccessToken = GenerateAccessToken(user, pairJti),
            RefreshToken = GenerateToken(refreshTokenClaims, refreshTokenExpires)
        };
    }

    private static List<Claim> GenerateAccessTokenClaims(User user, Guid jti)
    {
        return new List<Claim>()
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.UserRole.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, jti.ToString())
        };
    }

    private static List<Claim> GenerateRefreshTokenClaims(User user, Guid jti)
    {
        return new List<Claim>()
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, "RefreshToken"),
            new Claim(JwtRegisteredClaimNames.Jti, jti.ToString())
        };
    }

    private string GenerateToken(IEnumerable<Claim> claims, DateTime expires)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var tokenOptions = new JwtSecurityToken(
            claims: claims,
            issuer: _issuer,
            audience: _audience,
            expires: expires,
            signingCredentials: credentials
        );

        var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        return tokenString;
    }

    public string GenerateAccessToken(User user, Guid pairJti)
    {
        var tokenExpires = DateTime.UtcNow.AddMinutes(_accessTokenLifetimeMins);
        var tokenClaims = GenerateAccessTokenClaims(user, pairJti);

        return GenerateToken(tokenClaims, tokenExpires);
    }

    public string GenerateAccessTokenInExchangeForRefreshToken(string refreshToken)
    {
        var principal = GetPrincipalFromRefreshToken(refreshToken);

        // extract user name from principal claims
        var username = principal.Identity?.Name;

        // get user from db
        var user = _dbContext.Users.SingleOrDefault(u => u.Username == username);

        if (user is null)
            throw new SecurityTokenException("Invalid token");

        var accessTokenExpirationDate = DateTime.UtcNow.AddMinutes(_accessTokenLifetimeMins);
        var newJwtToken = GenerateAccessToken(user, Guid.NewGuid());

        return newJwtToken;
    }

    // TODO: Add more checks
    private ClaimsPrincipal GetPrincipalFromRefreshToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key)),
            
            ValidateIssuer = true,
            ValidIssuer = _issuer,
            
            ValidateAudience = true,
            ValidAudience = _audience,

            ValidateLifetime = true, // Here we are (not) saying that we don't care about the token's expiration 
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler
            .ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
        var jwtSecurityToken = securityToken as JwtSecurityToken;

        if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            throw new SecurityTokenException("Invalid token");

        return principal;
    }
}