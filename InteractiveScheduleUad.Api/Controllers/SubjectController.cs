using InteractiveScheduleUad.Api.Extensions;
using InteractiveScheduleUad.Api.Models;
using InteractiveScheduleUad.Api.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace InteractiveScheduleUad.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SubjectController : ControllerBase
{
    private readonly ISubjectService _subjectService;

    public SubjectController(ISubjectService subjectService)
    {
        _subjectService = subjectService;
    }

    // GET: api/<SubjectController>
    /// <summary>
    /// Retrieves all subjects
    /// </summary>
    /// <response code="200">Success - Returns an array of subjects</response>
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(IEnumerable<Subject>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IEnumerable<Subject>>> Get()
    {
        var result = await _subjectService.GetAllAsync();

        if (result.IsFailed)
            return result.Errors.First().ToObjectResult();
        else
            return Ok(result.Value);
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