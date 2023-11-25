﻿using FluentResults;
using InteractiveScheduleUad.Api.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractiveScheduleUad.E2ETests.Extensions;

internal static class RestClientExtensions
{
    // checks whether resource exists and returns it. If the resource doesn't exist, creates and returns it.
    // relies on POST endpoint returning the newly-created object
    public static EntityT EnsureExists<EntityT, PostPayloadT>
        (this RestClient client,
        string getEndpoint,
        string? postEndpoint,
        PostPayloadT postPayload,
        Predicate<EntityT> compareF)
         where PostPayloadT : class
    {
        postEndpoint ??= getEndpoint;

        var allEntities = client.GetJson<List<EntityT>>(getEndpoint);
        var matchingEntityFromDb = allEntities.Find(compareF);

        EntityT entityToReturn;
        if (matchingEntityFromDb is null)
        {
            entityToReturn = client.PostJson<PostPayloadT, EntityT>(postEndpoint, postPayload);
        }
        else
        {
            entityToReturn = matchingEntityFromDb;
        }

        return entityToReturn;
    }
}