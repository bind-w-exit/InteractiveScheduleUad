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

    private static async Task<RestClient> GetTeacherClient()
    {
        var config = await ApiConfigRetriever.GetBasePathAndAccessToken();

        var authenticator = new JwtAuthenticator(config.AccessToken);
        var options = new RestClientOptions(config.BasePath)
        {
            Authenticator = authenticator
        };
        var teacherApiClient = new RestClient(options);

        return teacherApiClient;
    }

    // runs before all tests
    public async Task InitializeAsync()
    {
        var _client = await GetTeacherClient();
        client = _client;
    }

    // runs after all tests
    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }

    [Fact]
    public async Task PostTeacher_ReturnsBackTeacherObject()
    {
        // Arrange
        var lastName = "Test Teacher Last Name";
        var teacher = new TeacherForWriteDto()
        {
            FirstName = "Test Teacher Name",
            LastName = lastName,
            Email = "testTeacherEmail@gmail.com"
        };

        var request = new RestRequest("Teacher").AddJsonBody(teacher);

        // Act

        var response = await client.PostAsync<Teacher>(request);

        // Assert
        Assert.Equal(teacher.FirstName, response.FirstName);
        Assert.Equal(teacher.LastName, response.LastName);
    }

    [Fact]
    public async Task GetAllTeachers_ReturnsArray()
    {
        // Arrange
        var request = new RestRequest("Teacher");

        // Act
        var response = await client.GetAsync<List<Teacher>>(request);

        // Assert
        Assert.IsType<List<Teacher>>(response);
    }
}