using InteractiveScheduleUad.Api.Models;
using InteractiveScheduleUad.Api.Models.Dtos;
using InteractiveScheduleUad.E2ETests.Extensions;
using InteractiveScheduleUad.E2ETests.Models;
using InteractiveScheduleUad.E2ETests.Utils;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;

namespace InteractiveScheduleUad.E2ETests;

public class TeacherTests : IAsyncLifetime
{
    private RestClient client = null;

    private string teacherEndpoint = "Teacher";
    private string teacherDepartmentEndpoint = "TeacherDepartment";

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

        var request = new RestRequest(teacherEndpoint);
        var response = await client.DeleteAsync(request);

        // delete all teacher departments

        request = new RestRequest(teacherDepartmentEndpoint);
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
        var getResponse = client.GetJson<TeacherForReadDto>($"{teacherEndpoint}/{response.Id}");

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
        var response = client.GetJson<List<TeacherForReadDto>>(teacherEndpoint);
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
        // Full name = Прізвище Ім'я По батькові
        var nameBits = rawTeacher.FullName.Split(' ');
        var lastName = nameBits.First();
        var firstName = nameBits[1];
        var middleName = nameBits.Length == 3 ? nameBits[2] : null; // aka patronymic

        var email = rawTeacher.Email;

        // ensure that department exists

        TeacherDepartmentForWriteDto newDepartment = new()
        {
            Name = rawTeacher.DepartmentFullName,
            Abbreviation = rawTeacher.DepartmentAbbreviation,
            Link = rawTeacher.DepartmentLink
        };
        var department = client.EnsureExists<Department, TeacherDepartmentForWriteDto>
            (teacherDepartmentEndpoint, null, newDepartment, (d) => d.Name == newDepartment.Name);
        var departmentId = department.Id;

        // construct teacher object

        var teacher = new TeacherForWriteDto()
        {
            FirstName = firstName,
            LastName = lastName,
            MiddleName = middleName,
            Email = email,
            Qualifications = rawTeacher.Qualification,
            DepartmentId = departmentId
        };

        return client.PostJson<TeacherForWriteDto, Teacher>(teacherEndpoint, teacher);
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