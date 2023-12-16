using InteractiveScheduleUad.Api.Models;
using InteractiveScheduleUad.Api.Models.Dtos;
using InteractiveScheduleUad.Core.Utils;
using InteractiveScheduleUad.Core.Extensions;
using InteractiveScheduleUad.E2ETests.Models;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using System.Net;
using Xunit.Sdk;
using InteractiveScheduleUad.E2ETests.Constants;
using InteractiveScheduleUad.E2ETests.UserActions;

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

        // delete all schedule lessons
        var scheduleLessonsRequest = new RestRequest(ApiEndpoints.scheduleLessonsEndpoint);
        await client.DeleteAsync(scheduleLessonsRequest);

        // delete all groups
        var request = new RestRequest(ApiEndpoints.groupsEndpoint);
        await client.DeleteAsync(request);

        // delete all lessons
        var lessonsRequest = new RestRequest(ApiEndpoints.lessonsEndpoint);
        await client.DeleteAsync(lessonsRequest);

        // delete all teachers
        var teachersRequest = new RestRequest(ApiEndpoints.teachersEndpoint);
        await client.DeleteAsync(teachersRequest);
    }

    // runs after all tests
    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }

    [Fact]
    public void CreatingStudentGroup_CompletesAsExpected()
    {
        // Arrange
        // the payload has to be wrapped in quotes
        string groupName = "ІСТ-5";
        var groupForWrite = new StudentsGroupForWriteDto { Name = groupName };

        // Act
        var response = client.PostJson<StudentsGroupForWriteDto, StudentsGroupForReadDto>
            (ApiEndpoints.groupsEndpoint, groupForWrite);

        string getEndpoint = $"{ApiEndpoints.groupsEndpoint}/{response.Id}";
        var getResponse = client.GetJson<StudentsGroupForReadDto>(getEndpoint);

        // Assert
        Assert.Equal(groupName, response.Name);
        Assert.Equal(groupName, getResponse.Name);
    }

    [Fact]
    public void CreatingLesson_CompletesAsExpected()
    {
        var rawScheduleObj = ReadRawExampleScheduleFromFile();

        Day rawScheduleMonday = rawScheduleObj.monday;
        ScheduleClass rawScheduleClass = rawScheduleMonday.classes.First();

        LessonForReadDto responseData = CreateCompleteLesson(rawScheduleClass);

        var sameLessonViaGet = client.GetJson<LessonForReadDto>($"{ApiEndpoints.lessonsEndpoint}/{responseData.Id}");

        Assert.Equivalent(responseData, sameLessonViaGet);
        Assert.Equal(rawScheduleClass.teacher, sameLessonViaGet.Teacher.LastName);
        Assert.Equal(rawScheduleClass.room, sameLessonViaGet.Room.Name);
    }

    [Fact]
    public void CreatingScheduleLesson_CompletesAsExpected()
    {
        // schedule lesson is just like a regular lesson, but with context

        var rawScheduleObj = ReadRawExampleScheduleFromFile();

        Day rawScheduleMonday = rawScheduleObj.monday;
        ScheduleClass rawScheduleClass = rawScheduleMonday.classes.First();

        var responseData = CreateCompleteScheduleLesson(rawScheduleClass, "ІСТ-5", DayOfWeek.Monday);
        var sameLessonViaGet = client.GetJson<ScheduleLessonForReadDto>($"{ApiEndpoints.scheduleLessonsEndpoint}/{responseData.Id}");

        Assert.Equivalent(responseData, sameLessonViaGet);
    }

    [Fact]
    public void UpdatingLesson_CompletesAsExpected()
    {
        // modify first lesson to reference the same room as the second lesson

        // Arrange

        var rawScheduleObj = ReadRawExampleScheduleFromFile();
        var rawScheduleMonday = rawScheduleObj.monday;

        var rawScheduleClass = rawScheduleMonday.classes.First();
        var anotherRawScheduleClass = rawScheduleMonday.classes.Last();

        var lessonOne = CreateCompleteLesson(rawScheduleClass);
        var lessonTwo = CreateCompleteLesson(anotherRawScheduleClass);

        // Act

        var lessonForUpdate = new LessonForWriteDto
        {
            TeacherId = lessonOne.Teacher.Id,
            RoomId = lessonTwo.Room.Id,
            SubjectId = lessonOne.Subject.Id,
        };

        var lessonEndpoint = $"{ApiEndpoints.lessonsEndpoint}/{lessonOne.Id}";

        var response = client.PutJson(lessonEndpoint, lessonForUpdate);
        var modifiedLesson = client.GetJson<LessonForReadDto>(lessonEndpoint);

        // Assert
        Assert.NotNull(modifiedLesson);
        Assert.Equal(modifiedLesson.Room.Name, lessonTwo.Room.Name);
    }

    [Fact]
    public void UpdatingScheduleLesson_CompletesAsExpected()
    {
        // create two schedule lessons and modifies base lesson of the first one to reference the second one

        // Arrange
        var rawScheduleObj = ReadRawExampleScheduleFromFile();
        var rawScheduleMonday = rawScheduleObj.monday;
        var rawScheduleClass = rawScheduleMonday.classes.First();
        var anotherRawScheduleClass = rawScheduleMonday.classes.Last();

        var scheduleLessonOne = CreateCompleteScheduleLesson(rawScheduleClass, "ІСТ-5", DayOfWeek.Monday);
        var scheduleLessonTwo = CreateCompleteScheduleLesson(anotherRawScheduleClass, "ІСТ-6", DayOfWeek.Tuesday);

        var secondContext = scheduleLessonTwo.FullContext;
        var secondTimeContext = secondContext.TimeContext;

        var firstContext = scheduleLessonOne.FullContext;
        var firstTimeContext = firstContext.TimeContext;

        // Act

        // modify first schedule lesson to reference the base lesson of second schedule lesson

        // re-create full context
        var fullContext = new FullContextForWriteDto
        {
            StudentsGroupId = firstContext.StudentsGroup.Id,
            TimeContext = new TimeContextForWriteDto
            {
                LessonIndex = firstTimeContext.LessonIndex,
                WeekDay = firstTimeContext.WeekDay,
                WeekIndex = firstTimeContext.WeekIndex
            }
        };

        var updatedScheduleLessonOne = new ScheduleLessonForWriteDto
        {
            FullContext = fullContext,
            LessonId = scheduleLessonTwo.Lesson.Id
        };

        var lessonEndpoint = $"{ApiEndpoints.scheduleLessonsEndpoint}/{scheduleLessonOne.Id}";

        var response = client.PutJson(lessonEndpoint, updatedScheduleLessonOne);
        var modifiedScheduleLessonOne = client.GetJson<ScheduleLessonForReadDto>(lessonEndpoint);

        // Assert

        Assert.NotNull(modifiedScheduleLessonOne);
        Assert.Equal(modifiedScheduleLessonOne.Lesson.Id, scheduleLessonTwo.Lesson.Id);
    }

    [Fact]
    public void CreatingAllScheduleLessonsOfSpecificSchedule_CompletesAsExpected()
    {
        // to create a schedule, you need to:
        // 1. create a group
        // 2. create schedule lessons:
        // 2.1 create all teachers, subjects, rooms first.
        // 2.2 create base lessons
        // 2.3 create schedule lessons that map base schedules to full contexts: groups and time contexts

        // Arrange

        // read "raw" schedule from file
        var rawScheduleObj = ReadRawExampleScheduleFromFile();
        var groupName = "ІСТ-5";

        Day rawScheduleMonday = rawScheduleObj.monday;
        var rawScheduleMondayClasses = rawScheduleMonday.classes;

        // Act

        ScheduleActions.CreateAllScheduleLessonsOfSchedule(client, groupName, rawScheduleObj);

        // Assert

        // note: only monday classes are used in Assert part.

        var allLessons = client.GetJson<List<ScheduleLessonForReadDto>>(ApiEndpoints.scheduleLessonsEndpoint);
        var mondayLessons = allLessons.Where(lsn => lsn.FullContext.TimeContext.WeekDay == DayOfWeek.Monday);

        Assert.Equal(rawScheduleMondayClasses.Count, mondayLessons.Count());
        Assert.Equal(rawScheduleObj.GetAllClassesCount(), allLessons.Count);
    }

    // - - relationship tests - -

    [Fact]
    public void DeletingStudentGroup_CascadeDeletesAllDependentScheduleLessons()
    {
        // Arrange
        var rawScheduleObj = ReadRawExampleScheduleFromFile();
        var rawScheduleMonday = rawScheduleObj.monday;
        var rawScheduleClass = rawScheduleMonday.classes.First();
        var anotherRawScheduleClass = rawScheduleMonday.classes.Last();

        var scheduleLesson = CreateCompleteScheduleLesson(rawScheduleClass, "ІСТ-5", DayOfWeek.Monday);
        var differentGroupLesson = CreateCompleteScheduleLesson(anotherRawScheduleClass, "ІСТ-6", DayOfWeek.Tuesday);

        // Act
        var groupDeleteRequest = new RestRequest($"{ApiEndpoints.groupsEndpoint}/{scheduleLesson.FullContext.StudentsGroup.Id}");
        var groupDeleteResponse = client.Delete(groupDeleteRequest);

        // Assert
        Assert.Equal(HttpStatusCode.OK, groupDeleteResponse.StatusCode);

        var allLessons = client.GetJson<List<ScheduleLessonForReadDto>>(ApiEndpoints.scheduleLessonsEndpoint);
        Assert.DoesNotContain(allLessons, l => l.Id == scheduleLesson.Id);
        Assert.Contains(allLessons, l => l.Id == differentGroupLesson.Id);
    }

    [Fact]
    public void DeletingLesson_CascadeDeletesAllDependentScheduleLessons()
    {
        // Arrange
        var rawScheduleObj = ReadRawExampleScheduleFromFile();
        var rawScheduleMonday = rawScheduleObj.monday;
        var rawScheduleClass = rawScheduleMonday.classes.First();
        var anotherRawScheduleClass = rawScheduleMonday.classes.Last();

        var scheduleLesson = CreateCompleteScheduleLesson(rawScheduleClass, "ІСТ-5", DayOfWeek.Monday);
        var differentLesson = CreateCompleteScheduleLesson(anotherRawScheduleClass, "ІСТ-5", DayOfWeek.Monday);

        // Act
        var lessonDeleteRequest = new RestRequest($"{ApiEndpoints.lessonsEndpoint}/{scheduleLesson.Lesson.Id}");
        var lessonDeleteResponse = client.Delete(lessonDeleteRequest);

        // Assert
        // TODO: find better way to assert that the two lessons are not equivalent
        Assert.Throws<EquivalentException>(() =>
            {
                Assert.Equivalent(rawScheduleClass, anotherRawScheduleClass);
            }
        );

        Assert.Equal(HttpStatusCode.OK, lessonDeleteResponse.StatusCode);

        var allScheduleLessons = client.GetJson<List<ScheduleLessonForReadDto>>(ApiEndpoints.scheduleLessonsEndpoint);
        Assert.DoesNotContain(allScheduleLessons, l => l.Id == scheduleLesson.Id);
        Assert.Contains(allScheduleLessons, l => l.Id == differentLesson.Id);
    }

    [Fact]
    public void DeletingTeacher_DoesNotDeleteDependentLessons()
    {
        // Arrange
        var rawScheduleObj = ReadRawExampleScheduleFromFile();
        var rawScheduleMonday = rawScheduleObj.monday;
        var rawScheduleClass = rawScheduleMonday.classes.First();

        var scheduleLesson = CreateCompleteScheduleLesson(rawScheduleClass, "ІСТ-5", DayOfWeek.Monday);

        // Act
        var teacherDeleteRequest = new RestRequest($"{ApiEndpoints.teachersEndpoint}/{scheduleLesson.Lesson.Teacher.Id}");
        var teacherDeleteResponse = client.Delete(teacherDeleteRequest);

        // Assert
        Assert.Equal(HttpStatusCode.OK, teacherDeleteResponse.StatusCode);

        var allLessons = client.GetJson<List<ScheduleLessonForReadDto>>(ApiEndpoints.scheduleLessonsEndpoint);
        Assert.Contains(allLessons, l => l.Lesson.Id == scheduleLesson.Lesson.Id);
    }

    [Fact]
    public void DeletingRoom_DoesNotDeleteDependentLessons()
    {
        // Arrange
        var rawScheduleObj = ReadRawExampleScheduleFromFile();
        var rawScheduleMonday = rawScheduleObj.monday;
        var rawScheduleClass = rawScheduleMonday.classes.First();

        var scheduleLesson = CreateCompleteScheduleLesson(rawScheduleClass, "ІСТ-5", DayOfWeek.Monday);

        // Act
        var roomDeleteRequest = new RestRequest($"{ApiEndpoints.roomsEndpoint}/{scheduleLesson.Lesson.Room.Id}");
        var roomDeleteResponse = client.Delete(roomDeleteRequest);

        // Assert
        Assert.Equal(HttpStatusCode.OK, roomDeleteResponse.StatusCode);

        var allLessons = client.GetJson<List<ScheduleLessonForReadDto>>(ApiEndpoints.scheduleLessonsEndpoint);
        Assert.Contains(allLessons, l => l.Lesson.Id == scheduleLesson.Lesson.Id);
    }

    // - - test helpers - -

    // POSTS a complete lesson with related entities pre-created in advance
    private LessonForReadDto CreateCompleteLesson(ScheduleClass rawScheduleClass)
    {
        return ScheduleActions.CreateCompleteLesson(rawScheduleClass, client);
    }

    // POSTS a complete schedule lesson with related entities pre-created in advance
    private ScheduleLessonForReadDto CreateCompleteScheduleLesson(
        ScheduleClass rawScheduleClass, string groupName, DayOfWeek weekDay)
    {
        return ScheduleActions.CreateCompleteScheduleLesson(rawScheduleClass, groupName, weekDay, client);
    }

    private static ScheduleFile ReadRawExampleScheduleFromFile()
    {
        return ScheduleActions.ReadRawExampleScheduleFromFile();
    }
}