using Azure;
using InteractiveScheduleUad.Api.Models;
using RestSharp;

namespace InteractiveScheduleUad.Core.Extensions;

public static class RestClientExtensions
{
    // TODO: use filters in get request instead of filtering client-side

    // checks whether resource exists and returns it. If the resource doesn't exist, creates and returns it.
    // relies on POST endpoint returning the newly-created object
    public static EntityT EnsureExists<EntityT, PostPayloadT>
        (this RestClient client,
        string getEndpoint,
        string? postEndpoint,
        PostPayloadT postPayload,
        Predicate<EntityT> compareF)
         where PostPayloadT : class
         where EntityT : class
    {
        postEndpoint ??= getEndpoint;

        EntityT? matchingEntityFromDb = CheckIfExists(client, getEndpoint, compareF);
        EntityT entityToReturn = CreateIfNotExists(client, postEndpoint, postPayload, matchingEntityFromDb);

        return entityToReturn;
    }

    public static EntityT EnsureExists_FilterServerSide<EntityT, PostPayloadT>
        (this RestClient client,
        string getEndpoint,
        string? postEndpoint,
        PostPayloadT postPayload,
        string filter)
         where PostPayloadT : class
         where EntityT : class
    {
        postEndpoint ??= getEndpoint;

        EntityT? matchingEntityFromDb = CheckIfExists_FilterServerSide<EntityT>(client, getEndpoint, filter);
        EntityT entityToReturn = CreateIfNotExists(client, postEndpoint, postPayload, matchingEntityFromDb);

        return entityToReturn;
    }

    private static EntityT CreateIfNotExists<EntityT, PostPayloadT>(RestClient client, string? postEndpoint, PostPayloadT postPayload, EntityT? matchingEntityFromDb)
        where EntityT : class
        where PostPayloadT : class
    {
        return matchingEntityFromDb ??
                    client.PostJson<PostPayloadT, EntityT>(postEndpoint, postPayload)!;
    }

    private static EntityT? CheckIfExists<EntityT>(RestClient client, string getEndpoint, Predicate<EntityT> compareF)
        where EntityT : class
    {
        var allEntities = client.GetJson<List<EntityT>>(getEndpoint);
        var matchingEntityFromDb = allEntities?.Find(compareF);

        return matchingEntityFromDb;
    }

    private static EntityT? CheckIfExists_FilterServerSide<EntityT>(RestClient client, string getEndpoint, string filter)
    where EntityT : class
    {
        var request = new RestRequest(getEndpoint);
        //request.AddQueryParameter("range", "[0, 9]");
        //request.AddQueryParameter("sort", "[\"Id\", \"ASC\"]");
        request.AddQueryParameter("filter", filter);

        var entity = client.Get<List<EntityT>>(request);

        if (entity is null || entity.Count == 0)
        {
            return null;
        }

        return entity.First();
    }

    //private static CheckIfExists()
    //{
    //}
}