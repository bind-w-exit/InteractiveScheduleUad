using InteractiveScheduleUad.Api.Models.Dtos;
using InteractiveScheduleUad.Api.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace InteractiveScheduleUad.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SchedulesController : ControllerBase
{
    private readonly IWeekScheduleService _weekScheduleService;

    public SchedulesController(IWeekScheduleService weekScheduleService)
    {
        _weekScheduleService = weekScheduleService;
    }

    //// GET api/<SchedulesController>/5
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(StudentsGroupForWriteDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<StudentsGroupForWriteDto>> Get([FromQuery] int studentsGroupId, [FromBody] WeekScheduleForWriteDto weekScheduleForWriteDto, [FromQuery] bool isSecondWeek = false)
    {
        StudentsGroupForWriteDto? studentsGroupForWriteDto = await _weekScheduleService.GetByIdAsync(studentsGroupId);

        if (studentsGroupForWriteDto is null)
            return BadRequest();
        else
            return Ok(studentsGroupForWriteDto);
    }

    // POST api/<SchedulesController>
    [HttpPost]
    [ProducesResponseType(typeof(WeekScheduleForReadDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<WeekScheduleForReadDto>> Post([FromQuery] int studentsGroupId, [FromBody] WeekScheduleForWriteDto weekScheduleForWriteDto, [FromQuery] bool isSecondWeek = false)
    {
        WeekScheduleForReadDto? weekScheduleForReadDto = await _weekScheduleService.CreateAsync(studentsGroupId, weekScheduleForWriteDto, isSecondWeek);

        if (weekScheduleForReadDto is null)
            return BadRequest();
        else
            return Ok(weekScheduleForReadDto);
    }

    //// DELETE api/<SchedulesController>/5
    [HttpDelete("{id}")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult> Delete([FromQuery] int studentsGroupId, [FromQuery] bool isSecondWeek = false)
    {
        var success = await _weekScheduleService.DeleteAsync(studentsGroupId, isSecondWeek);

        if (success)
            return Ok();
        else
            return BadRequest();
    }
}