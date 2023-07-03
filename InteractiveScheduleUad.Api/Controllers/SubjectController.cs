using InteractiveScheduleUad.Api.Models;
using InteractiveScheduleUad.Api.Services.Contracts;
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
    [ProducesResponseType(typeof(IEnumerable<Subject>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IEnumerable<Subject>>> Get()
    {
        IEnumerable<Subject> subjects = await _subjectService.GetAllAsync();

        return Ok(subjects);
    }

    // GET api/<SubjectController>/5
    /// <summary>
    /// Retrieves a subject by its ID
    /// </summary>
    /// <param name="id">Subjects ID</param>
    /// <response code="200">Success - Returns the subject with the specified ID</response>
    /// <response code="404">NotFound - Subject with the specified ID was not found</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Subject), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public async Task<ActionResult<Subject>> Get(int id)
    {
        Subject? subject = await _subjectService.GetByIdAsync(id);

        if (subject is null)
            return NotFound("Subject with the specified ID was not found");
        else
            return Ok(subject);
    }

    // POST api/<SubjectController>
    /// <summary>
    /// Creates a new subject
    /// </summary>
    /// <param name="subjectName">The subject name</param>
    /// <response code="201">Created - Returns the created subject</response>
    /// <response code="400">BadRequest - One or more validation errors occurred</response>
    [HttpPost]
    [ProducesResponseType(typeof(Subject), (int)HttpStatusCode.Created)]
    public async Task<ActionResult<Subject>> Post([FromBody] string subjectName)
    {
        Subject subject = await _subjectService.CreateAsync(subjectName);

        return CreatedAtAction(nameof(Get), new { id = subject.Id }, subject);
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
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public async Task<ActionResult> Put(int id, [FromBody] string newSubjectName)
    {
        bool success = await _subjectService.UpdateAsync(id, newSubjectName);

        if (!success)
            return NotFound("Subject with the specified ID was not found");
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
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public async Task<ActionResult> Delete(int id)
    {
        bool success = await _subjectService.DeleteAsync(id);

        if (!success)
            return NotFound("Subject with the specified ID was not found");
        else
            return Ok();
    }
}