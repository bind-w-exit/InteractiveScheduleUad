using InteractiveScheduleUad.Api.Models.Dtos;
using InteractiveScheduleUad.Api.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace InteractiveScheduleUad.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ScheduleController : ControllerBase
{
    private readonly IWeekScheduleService _weekScheduleService;

    public ScheduleController(IWeekScheduleService weekScheduleService)
    {
        _weekScheduleService = weekScheduleService;
    }

    // GET api/<ScheduleController>/5
    /// <summary>
    /// Retrieves a students group two weeks schedule by group ID
    /// </summary>
    /// <param name="studentsGroupId">Students group ID</param>
    /// <response code="200">Success - Returns the two weeks schedule with the specified students group ID</response>
    /// <response code="404">NotFound - Students group with the specified ID was not found</response>
    [HttpGet("{studentsGroupId}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(StudentsGroupForWriteDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public async Task<ActionResult<StudentsGroupForWriteDto>> Get(int studentsGroupId)
    {
        StudentsGroupForWriteDto? studentsGroupForWriteDto = await _weekScheduleService.GetByIdAsync(studentsGroupId);

        if (studentsGroupForWriteDto is null)
            return NotFound("Students group with the specified ID was not found");
        else
            return Ok(studentsGroupForWriteDto);
    }

    // POST api/<ScheduleController>
    /// <summary>
    /// Update a one week schedule for students group
    /// </summary>
    /// <param name="studentsGroupId">Students group ID</param>
    /// <param name="weekSchedule">The one week schedule data</param>
    /// <param name="isSecondWeek">Specifies which week to update, the first or the second. By default, the first</param>
    /// <response code="201">Success - Successfully updated</response>
    /// <response code="400">BadRequest - One or more validation errors occurred</response>
    /// <response code="404">NotFound - Students group with the specified ID was not found</response>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(WeekScheduleForReadDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public async Task<ActionResult<WeekScheduleForReadDto>> Post([FromQuery] int studentsGroupId, [FromBody] WeekScheduleForWriteDto weekSchedule, [FromQuery] bool isSecondWeek = false)
    {
        WeekScheduleForReadDto? weekScheduleForReadDto = await _weekScheduleService.CreateAsync(studentsGroupId, weekSchedule, isSecondWeek);

        if (weekScheduleForReadDto is null)
            return NotFound("Students group with the specified ID was not found");
        else
            return Ok(weekScheduleForReadDto);
    }

    // DELETE api/<ScheduleController>/5
    /// <summary>
    /// Deletes a firs or second week schedule for students group
    /// </summary>
    /// <param name="studentsGroupId">Students group ID</param>
    /// <param name="isSecondWeek">Specifies which week to delete, the first or the second. By default, the first</param>
    /// <response code="200">Success - Successfully deleted</response>
    /// <response code="404">NotFound - Students group with the specified ID was not found</response>
    [HttpDelete]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public async Task<ActionResult> Delete([FromQuery] int studentsGroupId, [FromQuery] bool isSecondWeek = false)
    {
        bool success = await _weekScheduleService.DeleteAsync(studentsGroupId, isSecondWeek);

        if (!success)
            return NotFound("Students group with the specified ID was not found");
        else
            return Ok();
    }
}