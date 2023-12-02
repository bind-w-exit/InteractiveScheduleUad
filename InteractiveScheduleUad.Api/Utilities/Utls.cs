using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Globalization;

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
    public static IEnumerable<DbSetRecordT> FilterSortAndRangeDbSet<DbSetRecordT>(InteractiveScheduleUadApiDbContext _context, string range, string sort, string filter,
        out int rangeStart, out int rangeEnd) where DbSetRecordT : class
    {
        // Parse filter parameter
        var filterParams = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);

        // the filtering is client-side. Could be a bottleneck :c
        var filteredResults = _context.Set<DbSetRecordT>().AsEnumerable();
        if (filterParams.Count != 0)
        {
            var filterField = filterParams.Keys.First();
            var filterValue = filterParams.Values.First();

            filterField = Utls.ToTitleCase(filterField);

            if (filterField == "Id" && filterValue.GetType().Name == "JArray")
            {
                // get only records with Ids specified in filter value
                var filterValueAsJArray = (JArray)filterValue;
                var ids = filterValueAsJArray.ToObject<int[]>();
                filteredResults = filteredResults.Where(s => ids.Contains(Utls.GetPropertyValue<int>(s, "Id")));
                //var one = 1;
            }
            else
            {
                // filter records by filter field and value (value of filter field should be a string)
                filteredResults = filteredResults
                    .Where(s =>
                    Utls.GetPropertyValue<string>(s, filterField).ToLower()
                    .Contains(filterValue.ToString().ToLower()));
            }
        }

        // Parse sort parameter
        var sortParams = JsonConvert.DeserializeObject<string[]>(sort);
        var (sortField, sortOrder) = (sortParams[0], sortParams[1]);

        // TODO: make react-admin send fields in titlecase
        sortField = Utls.ToTitleCase(sortField);

        // sort records by sort field and order (value of sort field should be a string)
        object? keySelector(DbSetRecordT r) => Utls.GetPropertyValue<object>(r, sortField);
        IEnumerable<DbSetRecordT> sortedResults = sortOrder == "ASC" ?
            filteredResults.OrderBy(keySelector) :
            filteredResults.OrderByDescending(keySelector);

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

    // gets object property name and returns that property value
    public static ReturnT? GetPropertyValue<ReturnT>(object obj, string propertyName)
    {
        ReturnT? value = (ReturnT)(obj.GetType().GetProperty(propertyName)?.GetValue(obj, null));
        return value;
    }

    // converts a string to a titlecase
    public static string ToTitleCase(string str)
    {
        return char.ToUpper(str[0]) + str[1..];
    }
}