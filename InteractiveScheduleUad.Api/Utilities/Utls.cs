using AutoFilterer.Extensions;
using AutoFilterer.Types;
using Azure;
using InteractiveScheduleUad.Api.Models;
using InteractiveScheduleUad.Api.Models.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Globalization;
using System.Reflection.Metadata.Ecma335;

namespace InteractiveScheduleUad.Api.Utilities;

public static class Utls
{
    public static bool IsForeignKeyOrphaned(int? key, object? resolvedEntity)
    {
        return (key is not null && resolvedEntity is null);
    }

    // checks whether entity exists and returns it. If the entity doesn't exist, creates and returns it.
    public static EntityT EnsureExists<EntityT>
        (DbSet<EntityT> dbSet, EntityT entity, Func<EntityT, bool> compareF)
         where EntityT : class
    {
        var matchingEntityFromDb = dbSet.Where(compareF).FirstOrDefault();

        EntityT entityToReturn;

        if (matchingEntityFromDb is null)
        {
            var addedEntity = dbSet.Add(entity);
            return addedEntity.Entity;
        }
        else
        {
            entityToReturn = matchingEntityFromDb;
        }

        return entityToReturn;
    }

    /// <summary>
    /// First filters, then sorts, and finally extracts a range of records from the sorted results.
    /// The operations are required by react-admin
    /// </summary>
    public static IEnumerable<DbSetRecordT> FilterSortAndRangeDbSet<DbSetRecordT, FilterT>(InteractiveScheduleUadApiDbContext _context,
        string range, string sort, string filter,
        out int rangeStart, out int rangeEnd) where DbSetRecordT : Entity where FilterT : FilterBase
    {
        //Debug.WriteLine($"filter: {filter}", "FilterSortAndRangeDbSet");

        var records = _context.Set<DbSetRecordT>();
        IQueryable<DbSetRecordT> filteredResults;
        // Parse filter parameter. It can be either a regular filter or a filter by id set
        try
        {
            IdSetFilter filterObject = JsonConvert.DeserializeObject<IdSetFilter>(filter);
            var ids = filterObject.Id ?? throw new JsonSerializationException();
            filteredResults = records.Where(r => ids.Contains(r.Id));
        }
        catch (JsonSerializationException)
        {
            // filter is not an id set filter
            var filterDto = JsonConvert.DeserializeObject<FilterT>(filter);
            filteredResults = _context.Set<DbSetRecordT>().ApplyFilter(filterDto);
        }
        var filteredEnumerableResults = filteredResults.AsEnumerable(); // to materialize the query

        // Parse sort parameter
        var sortParams = JsonConvert.DeserializeObject<string[]>(sort);
        var (sortField, sortOrder) = (sortParams[0], sortParams[1]);

        // TODO: make react-admin send fields in titlecase
        sortField = Utls.ToTitleCase(sortField);

        // sort records by sort field and order (value of sort field should be a string)
        object? keySelector(DbSetRecordT r) => Utls.GetPropertyChainValue<object>(r, sortField);
        IEnumerable<DbSetRecordT> sortedResults = sortOrder == "ASC" ?
            filteredEnumerableResults.OrderBy(keySelector) :
            filteredEnumerableResults.OrderByDescending(keySelector);

        // Parse range parameter
        var rangeParams = JsonConvert.DeserializeObject<int[]>(range);
        (rangeStart, rangeEnd) = (rangeParams[0], rangeParams[1]);

        // extract the range of records from the sorted results
        var rangeLength = rangeEnd - rangeStart + 1;
        var resultsRange = sortedResults.Skip(rangeStart).Take(rangeLength);

        return resultsRange;
    }

    // React admin requires the total count of items to be returned in a custom header
    public static void AddContentRangeHeader(int rangeStart, int rangeEnd, int totalCount,
    ControllerContext context, HttpResponse response)
    {
        var resourceName = context.ActionDescriptor.ControllerName;

        var totalCountHeader = $"{resourceName} {rangeStart}-{rangeEnd}/{totalCount}";
        response.Headers.Add("Content-Range", totalCountHeader);
    }

    public static ReturnT GetPropertyChainValue<ReturnT>(object obj, string propertyChain)
    {
        var propertyNames = propertyChain.Split('.', StringSplitOptions.RemoveEmptyEntries);
        var propertyValue = obj;

        foreach (var propertyName in propertyNames)
        {
            var propertyNameTitlecased = Utls.ToTitleCase(propertyName);
            propertyValue = Utls.GetPropertyValue<object>(propertyValue, propertyNameTitlecased);
        }

        return (ReturnT)propertyValue;
    }

    // gets access to object attribute by that attribute string name
    public static ReturnT? GetPropertyValue<ReturnT>(object obj, string propertyName)
    {
        ReturnT? value = (ReturnT)(obj.GetType().GetProperty(propertyName)?.GetValue(obj, null));
        return value;
    }

    public static bool IsPropertyChain(string text)
    {
        return text.Contains('.');
    }

    // converts a string to a titlecase
    public static string ToTitleCase(string str)
    {
        return char.ToUpper(str[0]) + str[1..];
    }
}