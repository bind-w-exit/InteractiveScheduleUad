using InteractiveScheduleUad.Api.Models;
using InteractiveScheduleUad.Api.Models.Dtos;
using InteractiveScheduleUad.E2ETests.Extensions;
using InteractiveScheduleUad.E2ETests.Utils;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractiveScheduleUad.E2ETests;

public class ReactAdminCompatibilityTests : IAsyncLifetime
{
    private RestClient client = null;
    private const string subjectsEndpoint = "Subject";

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

        // verify that results are filtered
        Assert.All(response, (s) => Assert.Equal(subject, s.Name));
    }
}