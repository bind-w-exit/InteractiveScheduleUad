using InteractiveScheduleUad.Api.Models;
using InteractiveScheduleUad.Api.Models.Dtos;
using InteractiveScheduleUad.E2ETests.Utils;
using RestSharp;
using RestSharp.Authenticators;

namespace InteractiveScheduleUad.E2ETests;

public class TeacherTests : IAsyncLifetime
{
    private RestClient client = null;

    public TeacherTests()
    {
    }

    private static async Task<RestClient> GetAuthenticatedClient()
    {
        var config = await ApiConfigRetriever.GetBasePathAndAccessToken();

        var authenticator = new JwtAuthenticator(config.AccessToken);
        var options = new RestClientOptions(config.BasePath)
        {
            Authenticator = authenticator
        };
        var authenticatedClient = new RestClient(options);

        return authenticatedClient;
    }

    // runs before all tests
    public async Task InitializeAsync()
    {
        var _client = await GetAuthenticatedClient();
        client = _client;
    }

    // runs after all tests
    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }

    // creates a teacher, then GETs it and checks whether it's the same as the one POSTed
    [Fact]
    public void PostTeacher_CreatesTeacher()
    {
        // Arrange
        var lastName = "Test Teacher Last Name";
        var teacher = new TeacherForWriteDto()
        {
            FirstName = "Test Teacher Name",
            LastName = lastName,
            Email = "testTeacherEmail@gmail.com"
        };

        // Act
        var response = client.PostJson<TeacherForWriteDto, Teacher>("Teacher", teacher);
        var getResponse = client.GetJson<Teacher>($"Teacher/{response.Id}");

        // Assert
        Assert.Equal(teacher.FirstName, response.FirstName);
        Assert.Equal(teacher.LastName, response.LastName);

        Assert.NotNull(getResponse);
        Assert.Equal(teacher.FirstName, getResponse.FirstName);
    }

    [Fact]
    public async Task GetAllTeachers_ReturnsArray()
    {
        // Act
        var response = await client.GetJsonAsync<List<Teacher>>("Teacher");

        // Assert
        Assert.IsType<List<Teacher>>(response);
    }
}