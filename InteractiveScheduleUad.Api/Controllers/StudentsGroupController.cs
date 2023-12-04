using InteractiveScheduleUad.Api.Controllers.Contracts;
using InteractiveScheduleUad.Api.Extensions;
using InteractiveScheduleUad.Api.Mappers;
using InteractiveScheduleUad.Api.Models;
using InteractiveScheduleUad.Api.Models.Dtos;
using InteractiveScheduleUad.Api.Models.Filters;
using InteractiveScheduleUad.Api.Services.Contracts;
using InteractiveScheduleUad.Api.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace InteractiveScheduleUad.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StudentsGroupController : ControllerBase, IReactAdminCompatible<StudentsGroupForReadDto>
{
    private readonly IStudentsGroupService _studentsGroupService;
    private readonly InteractiveScheduleUadApiDbContext _context;

    public StudentsGroupController(IStudentsGroupService studentsGroupService, InteractiveScheduleUadApiDbContext context)
    {
        _studentsGroupService = studentsGroupService;
        _context = context;
    }

    /// <summary>
    /// Retrieves a list of students groups.
    /// </summary>
    /// <response code="200">Success - Returns an array of students groups</response>
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(IEnumerable<StudentsGroupForReadDto>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IEnumerable<StudentsGroupForReadDto>>> GetList(
        [FromQuery] string range = "[0, 999999]",
        [FromQuery] string sort = "[\"Id\", \"ASC\"]",
        [FromQuery] string filter = "{}")
    {
        var resultsRange = Utls
            .FilterSortAndRangeDbSet<StudentsGroup, StudentsGroupForReadDtoFilter>(
            _context,
            range, sort, filter,
            out int rangeStart, out int rangeEnd);

        var totalCount = _context.StudentsGroups.Count();
        Utls.AddContentRangeHeader(
            rangeStart, rangeEnd, totalCount,
            ControllerContext, Response);

        var resultsForRead = resultsRange.Select(StudentsGroupMapper.StudentsGroupToStudentsGroupForReadDto);

        return Ok(resultsForRead);
    }

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
        var result = await _studentsGroupService.GetByIdAsync(id);

        if (result.IsFailed)
            return result.Errors.First().ToObjectResult();
        else
            return Ok(result.Value);
    }

    /// <summary>
    /// Creates a new students group
    /// </summary>
    /// <param name="groupForWrite">The students group name</param>
    /// <response code="201">Created - Returns the created students group</response>
    /// <response code="400">BadRequest - One or more validation errors occurred</response>
    [HttpPost]
    //[Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(StudentsGroupForReadDto), (int)HttpStatusCode.Created)]
    public async Task<ActionResult<StudentsGroupForReadDto>> Post([FromBody] StudentsGroupForWriteDto groupForWrite)
    {
        // creates a new group. May throw due to unique index constraint

        var newGroup = new StudentsGroup { Name = groupForWrite.Name };
        _context.StudentsGroups.Add(newGroup);

        await _context.SaveChangesAsync();

        var newGroupForRead = StudentsGroupMapper.StudentsGroupToStudentsGroupForReadDto(newGroup);
        return Ok(newGroupForRead);
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

    /// <summary>
    /// Deletes all groups
    /// </summary>
    /// <response code="200">Success - Successfully deleted</response>
    [HttpDelete()]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<ActionResult> DeleteAll()
    {
        await _context.StudentsGroups.ExecuteDeleteAsync();

        return Ok();
    }
}