using InteractiveScheduleUad.Api.Models;
using InteractiveScheduleUad.Api.Models.Dtos;
using InteractiveScheduleUad.Core.Extensions;
using InteractiveScheduleUad.E2ETests.Models;
using InteractiveScheduleUad.Core.Utils;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using InteractiveScheduleUad.E2ETests.Constants;

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

        // delete entities. Start with the topmost ones in the hierarchy

        // delete all teachers

        var request = new RestRequest(ApiEndpoints.teachersEndpoint);
        var response = await client.DeleteAsync(request);

        // delete all teacher departments

        request = new RestRequest(ApiEndpoints.teacherDepartmentEndpoint);
        response = await client.DeleteAsync(request);
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

        var rawTeachersObj = ReadRawTeachersFromFile();
        var rawTeacher = rawTeachersObj.First();

        // Act
        var response = CreateTeacher(rawTeacher);
        var getResponse = client.GetJson<TeacherForReadDto>($"{ApiEndpoints.teachersEndpoint}/{response.Id}");

        // Assert
        Assert.Equivalent(getResponse, response);
        Assert.Equal(getResponse.Department.Name, rawTeacher.DepartmentFullName);
        Assert.Equal(getResponse.Department.Abbreviation, rawTeacher.DepartmentAbbreviation);
    }

    [Fact]
    public void CreatingAllTeachers_CompletesAsExpected()
    {
        // Arrange

        var rawTeachersObj = ReadRawTeachersFromFile();

        // Act
        foreach (var rawTeacher in rawTeachersObj)
        {
            CreateTeacher(rawTeacher);
        }

        // Assert
        var response = client.GetJson<List<TeacherForReadDto>>(ApiEndpoints.teachersEndpoint);
        Assert.Equal(rawTeachersObj.Count, response.Count);
    }

    [Fact]
    public async Task GetAllTeachers_ReturnsArray()
    {
        // Act
        var response = await client.GetJsonAsync<List<Teacher>>("Teacher");

        // Assert
        Assert.IsType<List<Teacher>>(response);
    }

    private Teacher CreateTeacher(RawTeacher rawTeacher)
    {
        return UserActions.TeacherActions.CreateTeacher(rawTeacher, client);
    }

    private static TeachersFile ReadRawTeachersFromFile()
    {
        string pathToRawTeachersFile = @"Data\teachers.json";
        var rawTeachersText = File.ReadAllText(
            path: pathToRawTeachersFile,
            encoding: System.Text.Encoding.UTF8);
        var rawTeachesObj = JsonConvert.DeserializeObject<TeachersFile>(rawTeachersText);

        return rawTeachesObj;
    }
}