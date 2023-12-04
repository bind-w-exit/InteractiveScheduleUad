using AutoFilterer.Extensions;
using AutoFilterer.Types;
using Azure;
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
        out int rangeStart, out int rangeEnd) where DbSetRecordT : class where FilterT : FilterBase
    {
        Debug.WriteLine($"filter: {filter}", "FilterSortAndRangeDbSet");

        // Parse filter parameter
        var filterObject = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);

        var records = _context.Set<DbSetRecordT>();
        var filterDto = JsonConvert.DeserializeObject<FilterT>(filter);
        var filteredResults = _context.Set<DbSetRecordT>().ApplyFilter(filterDto);

        //if (filterObject.Count != 0 && false) // TODO: re-enable filtering
        //{
        //    var filterField = filterObject.Keys.First();
        //    var filterValue = filterObject.Values.First();

        //    filterField = Utls.ToTitleCase(filterField);

        //    if (filterField == "Id" && filterValue.GetType().Name == "JArray")
        //    {
        //        // get only records with Ids specified in filter value
        //        var filterValueAsJArray = (JArray)filterValue;
        //        var ids = filterValueAsJArray.ToObject<int[]>();
        //        filteredResults = filteredResults.Where(s => ids.Contains(Utls.GetPropertyChainValue<int>(s, "Id")));
        //        //var one = 1;
        //    }
        //    else
        //    {
        //        // filter records by filter field and value (value of filter field should be a string)
        //        filteredResults = filteredResults
        //            .Where(s =>
        //            Utls.GetPropertyChainValue<string>(s, filterField).ToLower()
        //            .Contains(filterValue.ToString().ToLower()));
        //    }
        //}

        // Parse sort parameter
        var sortParams = JsonConvert.DeserializeObject<string[]>(sort);
        var (sortField, sortOrder) = (sortParams[0], sortParams[1]);

        // TODO: make react-admin send fields in titlecase
        sortField = Utls.ToTitleCase(sortField);

        // sort records by sort field and order (value of sort field should be a string)
        object? keySelector(DbSetRecordT r) => Utls.GetPropertyChainValue<object>(r, sortField);
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

    /// <summary>
    /// First sorts, then extracts a range of records from the sorted results.
    /// The operations are required by react-admin
    /// </summary>
    //public static IEnumerable<DbSetRecordT> FilterSortAndRangeDbSet<DbSetRecordT>(InteractiveScheduleUadApiDbContext _context,
    //    string range, string sort, string filter,
    //    out int rangeStart, out int rangeEnd) where DbSetRecordT : class
    //{
    //    var filteredResults = _context.Set<DbSetRecordT>();

    //    // Parse sort parameter
    //    var sortParams = JsonConvert.DeserializeObject<string[]>(sort);
    //    var (sortField, sortOrder) = (sortParams[0], sortParams[1]);

    //    // TODO: make react-admin send fields in titlecase
    //    sortField = Utls.ToTitleCase(sortField);

    //    // sort records by sort field and order (value of sort field should be a string)
    //    object? keySelector(DbSetRecordT r) => Utls.GetPropertyChainValue<object>(r, sortField);
    //    IEnumerable<DbSetRecordT> sortedResults = sortOrder == "ASC" ?
    //        filteredResults.OrderBy(keySelector) :
    //        filteredResults.OrderByDescending(keySelector);

    //    // Parse range parameter
    //    var rangeParams = JsonConvert.DeserializeObject<int[]>(range);
    //    (rangeStart, rangeEnd) = (rangeParams[0], rangeParams[1]);

    //    // extract the range of records from the sorted results
    //    var rangeLength = rangeEnd - rangeStart + 1;
    //    var resultsRange = sortedResults.Skip(rangeStart).Take(rangeLength);

    //    return resultsRange;
    //}

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