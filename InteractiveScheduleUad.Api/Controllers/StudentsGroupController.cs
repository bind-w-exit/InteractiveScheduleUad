using InteractiveScheduleUad.Api.Extensions;
using InteractiveScheduleUad.Api.Models.Dtos;
using InteractiveScheduleUad.Api.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace InteractiveScheduleUad.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StudentsGroupController : ControllerBase
{
    private readonly IStudentsGroupService _studentsGroupService;

    public StudentsGroupController(IStudentsGroupService studentsGroupService)
    {
        _studentsGroupService = studentsGroupService;
    }

    // GET: api/<StudentsGroupController>
    /// <summary>
    /// Retrieves all students groups
    /// </summary>
    /// <response code="200">Success - Returns an array of students groups</response>
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(IEnumerable<StudentsGroupForReadDto>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IEnumerable<StudentsGroupForReadDto>>> Get()
    {
        var result = await _studentsGroupService.GetAllAsync();

        if (result.IsFailed)
            return result.Errors.First().ToObjectResult();
        else
            return Ok(result.Value);
    }

    // TODO: implement retrieval by Id
    // GET api/<StudentsGroupController>/5
    /// <summary>
    /// Retrieves a students group by its ID
    /// </summary>
    /// <param name="id">Students group ID</param>
    /// <response code="200">Success - Returns the students group with the specified ID</response>
    /// <response code="404">NotFound - Students group with the specified ID was not found</response>
    [HttpGet("{id}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(StudentsGroupForReadDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public async Task<ActionResult<StudentsGroupForReadDto>> Get(int id)
    {
        var result = await _studentsGroupService.GetAllAsync();

        if (result.IsFailed)
            return result.Errors.First().ToObjectResult();
        else
            return Ok(result.Value);
    }

    // POST api/<StudentsGroupController>
    /// <summary>
    /// Creates a new students group
    /// </summary>
    /// <param name="groupName">The students group name</param>
    /// <response code="201">Created - Returns the created students group</response>
    /// <response code="400">BadRequest - One or more validation errors occurred</response>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(StudentsGroupForReadDto), (int)HttpStatusCode.Created)]
    public async Task<ActionResult<StudentsGroupForReadDto>> Post([FromBody] string groupName)
    {
        var result = await _studentsGroupService.CreateAsync(groupName);

        if (result.IsFailed)
            return result.Errors.First().ToObjectResult();

        var createdStudentsGroup = result.Value;
        return CreatedAtAction(nameof(Get), new { id = createdStudentsGroup.Id }, createdStudentsGroup);
    }

    // PUT api/<StudentsGroupController>/5
    /// <summary>
    /// Updates an existing students group
    /// </summary>
    /// <param name="id">The ID of the students group to update</param>
    /// <param name="newGroupName">The updated students group name</param>
    /// <response code="200">Success - Successfully updated</response>
    /// <response code="400">BadRequest - One or more validation errors occurred</response>
    /// <response code="404">NotFound - Students group with the specified ID was not found</response>
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public async Task<ActionResult> Put(int id, [FromBody] string newGroupName)
    {
        var result = await _studentsGroupService.UpdateAsync(id, newGroupName);

        if (result.IsFailed)
            return result.Errors.First().ToObjectResult();
        else
            return Ok();
    }

    // DELETE api/<StudentsGroupController>/5
    /// <summary>
    /// Deletes a students group
    /// </summary>
    /// <param name="id">The ID of the students group to delete</param>
    /// <response code="200">Success - Successfully deleted</response>
    /// <response code="404">NotFound - Students group with the specified ID was not found</response>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public async Task<ActionResult> Delete(int id)
    {
        var result = await _studentsGroupService.DeleteAsync(id);

        if (result.IsFailed)
            return result.Errors.First().ToObjectResult();
        else
            return Ok();
    }
}