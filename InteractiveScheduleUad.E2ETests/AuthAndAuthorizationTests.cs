using Azure.Core;
using InteractiveScheduleUad.Api.Controllers;
using InteractiveScheduleUad.Api.Models;
using InteractiveScheduleUad.Api.Models.Dtos;
using InteractiveScheduleUad.E2ETests.Utils;
using Npgsql.Internal.TypeHandlers;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractiveScheduleUad.E2ETests;

public class AuthAndAuthorizationTests : IAsyncLifetime
{
    private RestClient client = null;

    private string authenticationEndpoint = "Auth";

    public AuthAndAuthorizationTests()
    {
    }

    private static async Task<RestClient> GetClientForAdmin()
    {
        var config = await ApiConfigRetriever.GetBasePathAndAccessToken();

        var client = GetClient(config.AccessToken, config.BasePath);

        return client;
    }

    private static RestClient GetClient(string accessToken, string basePath)
    {
        var authenticator = new JwtAuthenticator(accessToken);
        var options = new RestClientOptions(basePath)
        {
            Authenticator = authenticator
        };
        var authenticatedClient = new RestClient(options);

        return authenticatedClient;
    }

    // runs before all tests
    public async Task InitializeAsync()
    {
        var _client = await GetClientForAdmin();
        client = _client;
    }

    // runs after all tests
    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }

    [Fact]
    public void RegisteringNewUser_CompletesAsExpected()
    {
        // Arrange
        var newUser = new UserForRegisterDto
        {
            Username = Guid.NewGuid().ToString(),
            Password = Guid.NewGuid().ToString()
        };

        // Act
        var response = RegisterNewUser(newUser);

        // Assert
        Assert.Equal(newUser.Username, response.Username);
        Assert.Equal(newUser.UserRole, UserRole.Admin); // only admins, for now
    }

    [Fact]
    public void LoggingInForNewUser_CompletesAsExpected()
    {
        // Arrange
        var newUser = new UserForRegisterDto
        {
            Username = Guid.NewGuid().ToString(),
            Password = Guid.NewGuid().ToString()
        };

        // Act
        RegisterNewUser(newUser);

        var userForLogin = new UserForLoginDto
        {
            UserName = newUser.Username,
            Password = newUser.Password
        };

        var loginResponse = LoginUser(userForLogin);

        // Assert
        Assert.NotNull(loginResponse.AccessToken);
        Assert.NotNull(loginResponse.RefreshToken);
    }

    [Fact]
    public async Task RefreshingToken_CompletesAsExpected()
    {
        // registers a user, logs them in, then refreshes the access token and tries to access a protected endpoint

        // Arrange
        var newUserDto = new UserForRegisterDto
        {
            Username = Guid.NewGuid().ToString(),
            Password = Guid.NewGuid().ToString()
        };

        // Act
        RegisterNewUser(newUserDto);

        var userForLogin = new UserForLoginDto
        {
            UserName = newUserDto.Username,
            Password = newUserDto.Password
        };

        var loginResponse = LoginUser(userForLogin);
        var refreshToken = loginResponse.RefreshToken;

        // retrieve config from env to get base path
        var config = await ApiConfigRetriever.GetBasePathAndAccessToken();
        var userClient = GetClient(loginResponse.AccessToken, config.BasePath);

        var newAccessToken = userClient.PostJson<string, string>
            ($"{authenticationEndpoint}/RefreshToken", Utls.EncaseInQuotes(refreshToken));

        var refreshedClient = GetClient(newAccessToken, config.BasePath);

        // Assert

        Assert.NotEqual(newAccessToken, refreshToken);
        Assert.NotEqual(loginResponse.AccessToken, newAccessToken);

        Assert.NotNull(newAccessToken);
        Assert.IsType<string>(newAccessToken);
        Assert.False(string.IsNullOrEmpty(newAccessToken));

        // try to access a protected endpoint

        await AccessProtectedEndpoint(refreshedClient);
        Assert.True(true); // won't execute if the above line throws
    }

    [Fact(Skip = "There is no easy way to invalidate the token right away. But it gets expired on its own")]
    public async void LoggingOut_InvalidatesAccessToken()
    {
        // Arrange
        var newRandomlyGeneratedUser = new UserForRegisterDto
        {
            Username = Guid.NewGuid().ToString(),
            Password = Guid.NewGuid().ToString()
        };

        // Act
        RegisterNewUser(newRandomlyGeneratedUser);

        var userForLogin = new UserForLoginDto
        {
            UserName = newRandomlyGeneratedUser.Username,
            Password = newRandomlyGeneratedUser.Password
        };

        var loginResponse = LoginUser(userForLogin);

        // create client for the new user
        var config = await ApiConfigRetriever.GetBasePathAndAccessToken();
        var basePath = config.BasePath;

        var clientForNewUser = GetClient(loginResponse.AccessToken, basePath);

        LogoutUser(clientForNewUser);

        await Assert.ThrowsAsync<Exception>(
            async () => await AccessProtectedEndpoint(clientForNewUser));
    }

    private UserForReadDto RegisterNewUser(UserForRegisterDto userForRegisterDto)
    {
        var response = client.PostJson<UserForRegisterDto, UserForReadDto>($"{authenticationEndpoint}/Register", userForRegisterDto);
        return response;
    }

    private AuthenticatedResponse LoginUser(UserForLoginDto userForLoginDto)
    {
        var response = client.PostJson<UserForLoginDto, AuthenticatedResponse>($"{authenticationEndpoint}/Login", userForLoginDto);
        return response;
    }

    private void LogoutUser(RestClient client)
    {
        // logs out the user by hitting delete endpoint
        var request = new RestRequest($"{authenticationEndpoint}/Logout");
        client.Delete(request);
    }

    private async Task AccessProtectedEndpoint(RestClient client)
    {
        // POSTing to subject endpoint should require authentication.

        var protectedEndpoint = "Subject";
        var randomName = Guid.NewGuid().ToString();
        var subjectForWriteDto = new Subject
        {
            Name = randomName
        };

        var request = new RestRequest(protectedEndpoint).AddJsonBody(subjectForWriteDto);
        var response = client.Post(request);

        var responseContent = response.Content;

        //var response = client.PostJson<string, Subject>(protectedEndpoint, randomName);

        //return response;
    }
}