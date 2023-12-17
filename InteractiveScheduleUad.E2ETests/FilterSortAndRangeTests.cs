using InteractiveScheduleUad.Api.Models;
using InteractiveScheduleUad.Api.Models.Dtos;
using InteractiveScheduleUad.Api.Models.Filters;
using InteractiveScheduleUad.Core.Extensions;
using InteractiveScheduleUad.E2ETests.Models;
using InteractiveScheduleUad.Core.Utils;
using RestSharp;
using RestSharp.Authenticators;
using InteractiveScheduleUad.E2ETests.Constants;
using Newtonsoft.Json;

namespace InteractiveScheduleUad.E2ETests;

public class FilterSortAndRangeTests : IAsyncLifetime
{
    private RestClient client = null;

    // TODO: remove the fields and use ApiEndpoints directly

    private string lessonsEndpoint = ApiEndpoints.lessonsEndpoint;
    private string subjectsEndpoint = ApiEndpoints.subjectsEndpoint;

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
        var subjectName = "Web";
        var createdWebSubject = CreateSubject(subjectName);

        var request = new RestRequest(subjectsEndpoint);
        request.AddQueryParameter("range", "[0, 9]");
        request.AddQueryParameter("sort", "[\"Id\", \"ASC\"]");
        request.AddQueryParameter("filter", $"{{Name: {Utls.EncaseInQuotes(subjectName)}}}");

        var response = client.Get<List<Subject>>(request);

        Assert.True(response.Count > 0);
        // verify that results are filtered
        Assert.All(response, (s) => Assert.Equal(subjectName, s.Name));
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

    [Fact]
    public void GettingSeveralSubjects_CompletesAsExpected()
    {
        var firstSubjectName = "Web";
        var secondSubjectName = "Math";
        var createdWebSubject = CreateSubject(firstSubjectName);
        var createdMathSubject = CreateSubject(secondSubjectName);
        var createdWebSubjectId = createdWebSubject.Id;
        var createdMathSubjectId = createdMathSubject.Id;

        var ids = new int[] { createdWebSubjectId, createdMathSubjectId };

        var request = new RestRequest(subjectsEndpoint);
        var idFilter = new IdSetFilter()
        {
            Id = ids
        };
        var idFilterJson = Newtonsoft.Json.JsonConvert.SerializeObject(idFilter);
        request.AddQueryParameter("range", "[0, 9]");
        request.AddQueryParameter("sort", "[\"Id\", \"ASC\"]");
        request.AddQueryParameter("filter", idFilterJson);

        var response = client.Get<List<Subject>>(request);

        Assert.True(response.Count == 2);
    }

    private Subject CreateSubject(string subjectName)
    {
        //subjectEncased = Utls.EncaseInQuotes(subjectName);
        var subjectForWrite = new Subject { Name = subjectName };

        SubjectForReadDtoFilter subjectFilter = new() { Name = subjectName };
        string subjectFilterSerialized = JsonConvert.SerializeObject(subjectFilter);

        var subject = client.EnsureExists<Subject, Subject>(
                                  subjectsEndpoint, null, subjectForWrite, subjectFilterSerialized);

        return subject;
    }

    // POSTS a complete lesson with related entities pre-created in advance
    private LessonForReadDto CreateCompleteLesson(ScheduleClass rawScheduleClass)
    {
        return UserActions.ScheduleActions.CreateCompleteLesson(rawScheduleClass, client);
    }
}