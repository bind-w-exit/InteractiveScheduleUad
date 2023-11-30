﻿using InteractiveScheduleUad.Api.Extensions;
using InteractiveScheduleUad.Api.Models;
using InteractiveScheduleUad.Api.Services.Contracts;
using InteractiveScheduleUad.Api.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net;

namespace InteractiveScheduleUad.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SubjectController : ControllerBase
{
    private readonly ISubjectService _subjectService;
    private readonly InteractiveScheduleUadApiDbContext _context;

    public SubjectController(ISubjectService subjectService, InteractiveScheduleUadApiDbContext context)
    {
        _subjectService = subjectService;
        _context = context;
    }

    // GET: api/<SubjectController>
    /// <summary>
    /// Retrieves all subjects
    /// </summary>
    /// <response code="200">Success - Returns an array of subjects</response>
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(IEnumerable<Subject>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IEnumerable<Subject>>> GetList(
        [FromQuery] string range = $"[0, 999999]", 
        [FromQuery] string sort = "[\"Id\", \"ASC\"]",
        [FromQuery] string filter = "{}")
    {
        // Parse filter parameter
        var filterParams = JsonConvert.DeserializeObject<Dictionary<string, string>>(filter);
        IEnumerable<Subject> filteredResults = _context.Subjects.AsEnumerable();
        if (filterParams.Count != 0)
        {
            var filterField = filterParams.Keys.First();
            var filterValue = filterParams.Values.First();

            // filter subjects by filter field and value (filter field value should be a string)
            filteredResults = filteredResults
                .Where(s => Utls.GetPropertyValue<object>(s, filterField) == filterValue);
        }

        // Parse sort parameter
        var sortParams = JsonConvert.DeserializeObject<string[]>(sort);
        var sortField = sortParams[0];
        var sortOrder = sortParams[1];

        // TODO: make react-admin send fields in titlecase
        // titlecase sort field
        sortField = char.ToUpper(sortField[0]) + sortField[1..];

        // sort subjects by sort field and order (sort field value should be a string)
        IEnumerable<Subject> sortedResults;
        if (sortOrder == "ASC")
            sortedResults = filteredResults.OrderBy((s) => Utls.GetPropertyValue<object>(s, sortField));
        else
            sortedResults = filteredResults.OrderByDescending((s) => Utls.GetPropertyValue<object>(s, sortField));

        // Parse range parameter
        var rangeParams = JsonConvert.DeserializeObject<int[]>(range);
        var rangeStart = rangeParams[0];
        var rangeEnd = rangeParams[1];

        // extract the range of subjects from the sorted results
        var rangeLength = rangeEnd - rangeStart + 1;
        var resultsRange = sortedResults.Skip(rangeStart).Take(rangeLength);

        var totalCount = _context.Subjects.Count();
        AddContentRangeHeader(rangeStart, rangeEnd, totalCount);

        return Ok(resultsRange);

        //if (result.IsFailed)
        //    return result.Errors.First().ToObjectResult();
        //else
        //    return Ok(result.Value);
    }

    // React admin requires the total count of items to be returned in a custom header
    private void AddContentRangeHeader(int rangeStart, int rangeEnd, int totalCount)
    {
        var totalCountHeader = $"subject {rangeStart}-{rangeEnd}/{totalCount}";
        Response.Headers.Add("Content-Range", totalCountHeader);
    }

    // GET api/<SubjectController>/5
    /// <summary>
    /// Retrieves a subject by its ID
    /// </summary>
    /// <param name="id">Subjects ID</param>
    /// <response code="200">Success - Returns the subject with the specified ID</response>
    /// <response code="404">NotFound - Subject with the specified ID was not found</response>
    [HttpGet("{id}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(Subject), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public async Task<ActionResult<Subject>> Get(int id)
    {
        var result = await _subjectService.GetByIdAsync(id);

        if (result.IsFailed)
            return result.Errors.First().ToObjectResult();
        else
            return Ok(result.Value);
    }

    // POST api/<SubjectController>
    /// <summary>
    /// Creates a new subject
    /// </summary>
    /// <param name="subjectName">The subject name</param>
    /// <response code="201">Created - Returns the created subject</response>
    /// <response code="400">BadRequest - One or more validation errors occurred</response>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(Subject), (int)HttpStatusCode.Created)]
    public async Task<ActionResult<Subject>> Post([FromBody] string subjectName)
    {
        var result = await _subjectService.CreateAsync(subjectName);

        if (result.IsFailed)
            return result.Errors.First().ToObjectResult();

        var createdSubject = result.Value;
        return CreatedAtAction(nameof(Get), new { id = createdSubject.Id }, createdSubject);
    }

    // PUT api/<SubjectController>/5
    /// <summary>
    /// Updates an existing subject
    /// </summary>
    /// <param name="id">The ID of the subject to update</param>
    /// <param name="newSubjectName">The updated subject name</param>
    /// <response code="200">Success - Successfully updated</response>
    /// <response code="400">BadRequest - One or more validation errors occurred</response>
    /// <response code="404">NotFound - Subject with the specified ID was not found</response>
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public async Task<ActionResult> Put(int id, [FromBody] string newSubjectName)
    {
        var result = await _subjectService.UpdateAsync(id, newSubjectName);

        if (result.IsFailed)
            return result.Errors.First().ToObjectResult();
        else
            return Ok();
    }

    // DELETE api/<SubjectController>/5
    /// <summary>
    /// Deletes a subject
    /// </summary>
    /// <param name="id">The ID of the subject to delete</param>
    /// <response code="200">Success - Successfully deleted</response>
    /// <response code="404">NotFound - Subject with the specified ID was not found</response>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public async Task<ActionResult> Delete(int id)
    {
        var result = await _subjectService.DeleteAsync(id);

        if (result.IsFailed)
            return result.Errors.First().ToObjectResult();
        else
            return Ok();
    }
}