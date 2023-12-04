using InteractiveScheduleUad.Api.Models;
using InteractiveScheduleUad.Api.Models.Dtos;
using InteractiveScheduleUad.E2ETests.Extensions;
using InteractiveScheduleUad.E2ETests.Models;
using InteractiveScheduleUad.E2ETests.Utils;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractiveScheduleUad.E2ETests;

public class FilterSortAndRangeTests : IAsyncLifetime
{
    private RestClient client = null;

    private string teachersEndpoint = nameof(Teacher);
    private string lessonsEndpoint = nameof(Lesson);
    private string scheduleLessonsEndpoint = "ScheduleLesson";
    private string groupsEndpoint = nameof(StudentsGroup);
    private string subjectsEndpoint = nameof(Subject);
    private string roomsEndpoint = nameof(Room);

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

        // delete all schedule lessons
        //var scheduleLessonsRequest = new RestRequest(scheduleLessonsEndpoint);
        //await client.DeleteAsync(scheduleLessonsRequest);
    }

    // runs after all tests
    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }

    [Fact]
    public void GettingRangedSortedListOfSubjects_CompletesAsExpected()
    {
        var request = new RestRequest(subjectsEndpoint);
        request.AddQueryParameter("range", "[0, 9]");
        request.AddQueryParameter("sort", "[\"Id\", \"ASC\"]");
        request.AddQueryParameter("filter", "{}");

        var response = client.Get<List<Subject>>(request);

        Assert.Equal(10, response.Count);

        // verify that results are sorted
        var sorted = response.OrderBy(s => s.Id).ToList();
        for (int i = 0; i < sorted.Count; i++)
        {
            Assert.Equal(sorted[i].Id, response[i].Id);
        }
    }

    // filters by primitive property
    [Fact]
    public void GettingFilteredListOfSubjects_CompletesAsExpected()
    {
        // pre-create subject with name "Web"
        var subject = "Web";
        var subjectEncased = Utls.EncaseInQuotes(subject);
        client.EnsureExists
            <Subject, string>
            (subjectsEndpoint, null, subjectEncased, (s) => s.Name == subject);

        var request = new RestRequest(subjectsEndpoint);
        request.AddQueryParameter("range", "[0, 9]");
        request.AddQueryParameter("sort", "[\"Id\", \"ASC\"]");
        request.AddQueryParameter("filter", $"{{Name: {subjectEncased}}}");

        var response = client.Get<List<Subject>>(request);

        Assert.True(response.Count > 0);
        // verify that results are filtered
        Assert.All(response, (s) => Assert.Equal(subject, s.Name));
    }

    // filters by related entity property
    [Fact]
    public void GettingFilteredListOfLessons_CompletesAsExpected()
    {
        // pre-create a lesson
        var rawScheduleClass = new ScheduleClass
        {
            name = "Web",
            teacher = "Teacher",
            room = "Room"
        };

        var lesson = CreateCompleteLesson(rawScheduleClass);

        var request = new RestRequest(lessonsEndpoint);
        string filterQueryValue = @"{
          ""subject"":{
            ""name"": ""Web""
	        }
        }";
        request.AddQueryParameter("range", "[0, 9]");
        request.AddQueryParameter("sort", "[\"Id\", \"ASC\"]");
        request.AddQueryParameter("filter", filterQueryValue);

        var response = client.Get<List<LessonForReadDto>>(request);

        Assert.True(response.Count > 0);
        // verify that results are filtered
        Assert.All(response, (l) => Assert.Equal(lesson.Subject.Name, l.Subject.Name));
    }

    // note: copypasted from ScheduleTests
    // POSTS a complete lesson with related entities pre-created in advance
    private LessonForReadDto CreateCompleteLesson(ScheduleClass rawScheduleClass)
    {
        LessonForWriteDto requestBody;
        LessonForReadDto responseData;

        // pre-create necessary related entities (if they don't exist already)
        TeacherForWriteDto teacherForWrite = new() { LastName = rawScheduleClass.teacher, FirstName = "" };
        var teacher = client.EnsureExists<TeacherForReadDto, TeacherForWriteDto>(
            teachersEndpoint, null, teacherForWrite, (t) => t.LastName == rawScheduleClass.teacher);
        var teacherId = teacher.Id;

        var subject = client.EnsureExists<Subject, string>(
            subjectsEndpoint, null, Utls.EncaseInQuotes(rawScheduleClass.name), (s) => s.Name == rawScheduleClass.name);
        var subjectId = subject.Id;

        var roomForWrite = new RoomForWriteDto { Name = rawScheduleClass.room };
        var room = client.EnsureExists<Room, RoomForWriteDto>(
            roomsEndpoint, null, roomForWrite, (r) => r.Name == rawScheduleClass.room);
        var roomId = room.Id;

        // assemble final request
        requestBody = new()
        {
            RoomId = roomId,
            SubjectId = subjectId,
            TeacherId = teacherId
        };

        // POST the lesson with entities
        responseData = client.PostJson<LessonForWriteDto, LessonForReadDto>(lessonsEndpoint, requestBody);

        return responseData;
    }
}