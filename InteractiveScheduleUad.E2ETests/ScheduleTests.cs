using InteractiveScheduleUad.Api.Models;
using InteractiveScheduleUad.Api.Models.Dtos;
using InteractiveScheduleUad.E2ETests.Models;
using InteractiveScheduleUad.E2ETests.Utils;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace InteractiveScheduleUad.E2ETests;

public class ScheduleTests : IAsyncLifetime
{
    private RestClient client = null;

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

    [Fact]
    public async Task CreatingStudentGroup_CompletesAsExpected()
    {
        // Arrange
        // the payload has to be wrapped in quotes
        string groupName = "\"ІСТ-5\"";
        string endpoint = "StudentsGroup";

        // Act
        var response = await client.PostJsonAsync<string, StudentsGroupForReadDto>(endpoint, groupName);

        // Assert
        Assert.Equal(groupName, $"\"{response.GroupName}\"");
    }

    [Fact]
    public async Task CreatingSchedule_CompletesAsExpected()
    {
        // to create a schedule, you need to:
        // 1. create a group
        // 2. create a schedule:
        // 2.1 create all teachers, subjects, rooms first.
        // 2.2 create a schedule
        // 3. add the schedule to the group

        // arrange

        string pathToRawScheduleFile = @"Data\ІСТ-5.json";
        var rawScheduleText = await File.ReadAllTextAsync(pathToRawScheduleFile);
        var rawScheduleObj = JsonConvert.DeserializeObject<ScheduleFile>(rawScheduleText);

        Day rawScheduleMonday = rawScheduleObj.monday;

        var studentsGroup = await client.PostJsonAsync<string, StudentsGroupForReadDto>("StudentsGroup", "\"ІСТ-5\"");
        var studentsGroupId = studentsGroup.Id;

        // act

        // monday only
        var scheduleForWriteDto = new WeekScheduleForWriteDto()
        {
            Monday = rawScheduleObj.monday.classes.Select(RawClassToLessonForWriteDto),
        };

        bool isSecondWeek = false;
        string endpoint = $"Schedule?studentsGroupId={studentsGroupId}&isSecondWeek={isSecondWeek}";

        var createScheduleResult = client.PostJson<WeekScheduleForWriteDto, WeekScheduleForReadDto>(endpoint, scheduleForWriteDto);

        Console.WriteLine(createScheduleResult);

        // assert

        var firstLesson = createScheduleResult.Monday.First();
        var firstLessonRaw = rawScheduleMonday.classes.First();
        Assert.Equal(createScheduleResult.Monday.Count(), rawScheduleMonday.classes.Count);
        Assert.Equal(firstLesson.Sequence, firstLessonRaw.index);
        Assert.Equal(firstLesson.Room, firstLessonRaw.room);
        Assert.Equal(firstLesson.Subject, firstLessonRaw.name);

        var retrievedScheduleObj = client.GetJson<StudentsGroupForWriteDto>($"Schedule/{studentsGroupId}");
        var retrievedSchedule = retrievedScheduleObj.FirstWeekSchedule;

        Assert.NotNull(retrievedSchedule);
    }

    private LessonForWriteDto RawClassToLessonForWriteDto(Class classObj)
    {
        // TODO: check whether the entities already exist. Especially teachers as there is a separate data source for them

        Room createdRoom = client.PostJson<RoomForWriteDto, Room>("Room", new RoomForWriteDto() { Name = classObj.room });
        var createdRoomId = createdRoom.Id;

        var createdSubject = client.PostJson<string, Subject>("Subject", EncaseInQuotes(classObj.name));
        var createdSubjectId = createdSubject.Id;

        var teacherNameBits = classObj.teacher.Split(' ');
        var teacherLastName = teacherNameBits.Length != 0 ? teacherNameBits[0] : "";
        var teacherFirstName = teacherNameBits.Length == 2 ? teacherNameBits[1] : "";

        TeacherForWriteDto teacher = new()
        {
            FirstName = teacherFirstName,
            LastName = teacherLastName,
        };
        var createdTeacher = client.PostJson<TeacherForWriteDto, Teacher>("Teacher", teacher);
        var createdTeacherId = createdTeacher.Id;

        var lessonForWriteDto = new LessonForWriteDto()
        {
            Sequence = classObj.index,

            RoomId = createdRoomId,
            SubjectId = createdSubjectId,
            TeacherId = createdTeacherId,
        };

        return lessonForWriteDto;
    }

    public static string EncaseInQuotes(string text)
    {
        return $"\"{text}\"";
    }
}