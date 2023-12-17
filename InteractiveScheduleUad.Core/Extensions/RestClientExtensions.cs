using Azure;
using InteractiveScheduleUad.Api.Models;
using RestSharp;

namespace InteractiveScheduleUad.Core.Extensions;

public static class RestClientExtensions
{
    /// <summary>
    /// Checks whether resource exists and returns it. If the resource doesn't exist, creates and returns it.
    /// Relies on POST endpoint returning the newly-created object
    /// </summary>
    public static EntityT EnsureExists<EntityT, PostPayloadT>
        (this RestClient client,
        string getEndpoint,
        string? postEndpoint,
        PostPayloadT postPayload,
        string filter)
         where PostPayloadT : class
         where EntityT : class
    {
        postEndpoint ??= getEndpoint;

        EntityT? matchingEntityFromDb = CheckIfExists<EntityT>(client, getEndpoint, filter);
        EntityT entityToReturn = CreateIfNotExists(client, postEndpoint, postPayload, matchingEntityFromDb);

        return entityToReturn;
    }

    private static EntityT? CheckIfExists<EntityT>(RestClient client, string getEndpoint, string filter)
   where EntityT : class
    {
        var request = new RestRequest(getEndpoint);
        request.AddQueryParameter("filter", filter);

        var entity = client.Get<List<EntityT>>(request);

        if (entity is null || entity.Count == 0)
        {
            return null;
        }

        return entity.First();
    }

    private static EntityT CreateIfNotExists<EntityT, PostPayloadT>(RestClient client, string? postEndpoint, PostPayloadT postPayload, EntityT? matchingEntityFromDb)
        where EntityT : class
        where PostPayloadT : class
    {
        return matchingEntityFromDb ??
                    client.PostJson<PostPayloadT, EntityT>(postEndpoint, postPayload)!;
    }
}