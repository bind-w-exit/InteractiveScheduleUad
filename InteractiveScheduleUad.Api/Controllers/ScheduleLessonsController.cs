using InteractiveScheduleUad.Api.Extensions;
using InteractiveScheduleUad.Api.Models;
using InteractiveScheduleUad.Api.Models.Dtos;
using InteractiveScheduleUad.Api.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace InteractiveScheduleUad.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ScheduleLessonsController : ControllerBase
{
    private readonly IWeekScheduleService _weekScheduleService;

    private readonly InteractiveScheduleUadApiDbContext _context;

    public ScheduleLessonsController(IWeekScheduleService weekScheduleService, InteractiveScheduleUadApiDbContext context)
    {
        _weekScheduleService = weekScheduleService;
        _context = context;
    }

    // GET api/<ScheduleController>/5
    /// <summary>
    /// Retrieves all lessons that belong to the group that is specified by Id.
    /// </summary>
    /// <param name="studentsGroupId">Students group ID</param>
    /// <response code="200">Success - Returns the two weeks schedule with the specified students group ID</response>
    /// <response code="404">NotFound - Students group with the specified ID was not found</response>
    [HttpGet("{studentsGroupId}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(StudentsGroupWithSchedulesDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public ActionResult<IQueryable<ScheduleLesson>> Get(int studentsGroupId)
    {
        //var result = await _weekScheduleService.GetByIdAsync(studentsGroupId);
        // get all schedule lessons with students group Id

        var lessons = _context.ScheduleLessons.Where(l => l.StudentsGroupId == studentsGroupId);

        var result = new ActionResult<IQueryable<ScheduleLesson>>(lessons);

        return result;

        //if (result.IsFailed)
        //    return result.Errors.First().ToObjectResult();
        //else
        //    return Ok(result.Value);
    }

    // POST api/<ScheduleController>
    /// <summary>
    /// Creates new schedule lesson
    /// </summary>
    /// <param name="studentsGroupId">Students group ID</param>
    /// <param name="lessonId">Lesson Id</param>
    /// <param name="timeContext">Time context for the lesson</param>
    /// <response code="201">Success - Successfully updated</response>
    /// <response code="400">BadRequest - One or more validation errors occurred</response>
    /// <response code="404">NotFound - Students group with the specified ID was not found</response>
    [HttpPost]
    //[Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(WeekScheduleForReadDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public async Task<ActionResult<ScheduleLesson>> Post([FromQuery] int studentsGroupId, [FromQuery] int lessonId, [FromBody] TimeContextForWriteDto timeContext)
    {
        //var result = await _weekScheduleService.CreateAsync(studentsGroupId, weekSchedule, isSecondWeek);

        var fullTimeContext = new TimeContext()
        {
            LessonIndex = timeContext.LessonIndex,
            WeekDay = timeContext.WeekDay,
            WeekIndex = timeContext.WeekIndex,
        };

        var timeContextEntry = _context.TimeContexts.Add(fullTimeContext);

        await _context.SaveChangesAsync();
        var ctxId = _context.TimeContexts.OrderBy(r => r.Id).Last().Id;


        var newScheduleLesson = new ScheduleLesson()
        {
            LessonId = lessonId,
            StudentsGroupId = studentsGroupId,
            TimeContextId = ctxId
        };

        _context.ScheduleLessons.Add(newScheduleLesson);
        await _context.SaveChangesAsync();

        var result = new ActionResult<ScheduleLesson>(newScheduleLesson);

        //if (result.IsFailed)
        //    return result.Errors.First().ToObjectResult();
        //else
        //    return Ok(result.Value);

        return result;
    }

    // DELETE api/<ScheduleController>/5
    /// <summary>
    /// Deletes a first or second week schedule for students group
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
        var result = await _weekScheduleService.DeleteAsync(studentsGroupId, isSecondWeek);

        if (result.IsFailed)
            return result.Errors.First().ToObjectResult();
        else
            return Ok();
    }
}