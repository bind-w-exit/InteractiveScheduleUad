using InteractiveScheduleUad.Api.Models.Dtos;
using RestSharp;
using InteractiveScheduleUad.Api.Models;

namespace InteractiveScheduleUad.E2ETests.Utils;

public record Config(string BasePath, string AccessToken);

internal class ApiConfigRetriever
{
    public static async Task<Config> GetBasePathAndAccessToken()
    {
        // Load environment variables from .env file
        DotNetEnv.Env.TraversePath().Load();

        // Read secrets from environment variables
        string apiUserName = Environment.GetEnvironmentVariable("API_USER_NAME");
        string apiUserPassword = Environment.GetEnvironmentVariable("API_USER_PASSWORD");
        string basePath = Environment.GetEnvironmentVariable("BASE_PATH");

        if (string.IsNullOrEmpty(apiUserName) || string.IsNullOrEmpty(apiUserPassword) || string.IsNullOrEmpty(basePath))
            throw new Exception("One of the environment variables is missing");

        var options = new RestClientOptions(basePath);
        var authApi = new RestClient(
            options
        );

        async Task<(string accessToken, string refreshToken)> GetTokens()
        {
            UserForLoginDto userCreds = new()
            {
                UserName = apiUserName,
                Password = apiUserPassword
            };

            var request = new RestRequest("Auth/Login").AddJsonBody(userCreds);
            var response = await authApi.PostAsync<AuthenticatedResponse>(request);

            if (string.IsNullOrEmpty(response.AccessToken) || string.IsNullOrEmpty(response.RefreshToken))
                throw new Exception("One of the tokens is missing");

            return (response.AccessToken, response.RefreshToken);
        }

        var tokens = await GetTokens();
        string accessToken = tokens.accessToken;
        string refreshToken = tokens.refreshToken;

        var config = new Config(basePath, accessToken);

        return config;
    }
}