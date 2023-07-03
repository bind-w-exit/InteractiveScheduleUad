using InteractiveScheduleUad.Api.Models.Dtos;
using InteractiveScheduleUad.Api.Services.Contracts;
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
    [ProducesResponseType(typeof(IEnumerable<StudentsGroupForReadDto>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IEnumerable<StudentsGroupForReadDto>>> Get()
    {
        IEnumerable<StudentsGroupForReadDto> studentsGroups = await _studentsGroupService.GetAllAsync();

        return Ok(studentsGroups);
    }

    // GET api/<StudentsGroupController>/5
    /// <summary>
    /// Retrieves a students group by its ID
    /// </summary>
    /// <param name="id">Students group ID</param>
    /// <response code="200">Success - Returns the students group with the specified ID</response>
    /// <response code="404">NotFound - Students group with the specified ID was not found</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(StudentsGroupForReadDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public async Task<ActionResult<StudentsGroupForReadDto>> Get(int id)
    {
        StudentsGroupForReadDto? studentsGroup = await _studentsGroupService.GetByIdAsync(id);

        if (studentsGroup is null)
            return NotFound("Students group with the specified ID was not found");
        else
            return Ok(studentsGroup);
    }

    // POST api/<StudentsGroupController>
    /// <summary>
    /// Creates a new students group
    /// </summary>
    /// <param name="groupName">The students group name</param>
    /// <response code="201">Created - Returns the created students group</response>
    /// <response code="400">BadRequest - One or more validation errors occurred</response>
    [HttpPost]
    [ProducesResponseType(typeof(StudentsGroupForReadDto), (int)HttpStatusCode.Created)]
    public async Task<ActionResult<StudentsGroupForReadDto>> Post([FromBody] string groupName)
    {
        StudentsGroupForReadDto studentsGroupForReadDto = await _studentsGroupService.CreateAsync(groupName);

        return CreatedAtAction(nameof(Get), new { id = studentsGroupForReadDto.Id }, studentsGroupForReadDto);
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
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public async Task<ActionResult> Put(int id, [FromBody] string newGroupName)
    {
        bool success = await _studentsGroupService.UpdateAsync(id, newGroupName);

        if (!success)
            return NotFound("Students group with the specified ID was not found");
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
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public async Task<ActionResult> Delete(int id)
    {
        bool success = await _studentsGroupService.DeleteAsync(id);

        if (!success)
            return NotFound("Students group with the specified ID was not found");
        else
            return Ok();
    }
}