using InteractiveScheduleUad.Api.Controllers.Contracts;
using InteractiveScheduleUad.Api.Errors;
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

// TODO: re-enable authorizations on handlers

[Route("api/[controller]")]
[ApiController]
public class ScheduleLessonController : ControllerBase, IReactAdminCompatible<ScheduleLessonForReadDto>
{
    private readonly InteractiveScheduleUadApiDbContext _context;

    public ScheduleLessonController(InteractiveScheduleUadApiDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retrieves a list of schedule lessons.
    /// </summary>
    /// <response code="200">Success - Returns a list of schedule lessons</response>
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ScheduleLessonForReadDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public async Task<ActionResult<IEnumerable<ScheduleLessonForReadDto>>> GetList(
        [FromQuery] string range = "[0, 999999]",
        [FromQuery] string sort = "[\"Id\", \"ASC\"]",
        [FromQuery] string filter = "{}")
    {
        var resultsRange = Utls
           .FilterSortAndRangeDbSet<ScheduleLessonJunction, ScheduleLessonForReadDtoFilter>(
            _context,
            range, sort, filter,
            out int rangeStart, out int rangeEnd);

        var totalCount = _context.ScheduleLessonJunctions.Count();
        Utls.AddContentRangeHeader(
            rangeStart, rangeEnd, totalCount,
            ControllerContext, Response);

        var resultsForRead = resultsRange.Select(ScheduleLessonMapper.ScheduleLessonToScheduleLessonForRead);

        return Ok(resultsForRead);
    }

    /// <summary>
    /// Retrieves schedule lesson by Id.
    /// </summary>
    /// <param name="Id">Lesson Id</param>
    /// <response code="200">Success - Returns the two weeks schedule with the specified students group ID</response>
    /// <response code="404">NotFound - Students group with the specified ID was not found</response>
    [HttpGet("{Id}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ScheduleLessonForReadDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public ActionResult<ScheduleLessonForReadDto> GetById(int Id)
    {
        var lesson = _context.ScheduleLessonJunctions.Find(Id);

        if (lesson is null)
        {
            return NotFound();
        }

        var lessonForRead = ScheduleLessonMapper.ScheduleLessonToScheduleLessonForRead(lesson);

        var result = new ActionResult<ScheduleLessonForReadDto>(lessonForRead);

        return result;
    }

    /// <summary>
    /// Creates new schedule lesson
    /// </summary>
    /// <response code="201">Success - Successfully created a schedule lesson</response>
    /// <response code="400">BadRequest - One or more validation errors occurred</response>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ScheduleLessonForReadDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public async Task<ActionResult<ScheduleLessonForReadDto>> Post([FromBody] ScheduleLessonForWriteDto lessonForWrite)
    {
        var newLessonJunction = await CreateScheduleLessonJunction(lessonForWrite);

        await _context.ScheduleLessonJunctions.AddAsync(newLessonJunction);
        await _context.SaveChangesAsync();

        var lessonForReadDto = ScheduleLessonMapper.ScheduleLessonToScheduleLessonForRead(newLessonJunction);

        return Ok(lessonForReadDto);
    }

    /// <summary>
    /// Updates an existing schedule lesson
    /// </summary>
    /// <response code="200">Success - Successfully updated schedule lesson</response>
    /// <response code="404">Not Found</response>
    [HttpPut("{Id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ScheduleLessonJunction), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public async Task<ActionResult<ScheduleLessonForReadDto>> Update(int Id, [FromBody] ScheduleLessonForWriteDto lessonForWrite)
    {
        // get the lesson, but don't track it
        var lesson = _context.ScheduleLessonJunctions.AsNoTracking().FirstOrDefault(l => l.Id == Id);
        if (lesson == null)
        {
            return NotFound();
        }

        var newLessonJunction = await CreateScheduleLessonJunction(lessonForWrite);
        newLessonJunction.Id = Id;

        _context.ScheduleLessonJunctions.Update(newLessonJunction);
        await _context.SaveChangesAsync();

        var lessonForReadDto = ScheduleLessonMapper.ScheduleLessonToScheduleLessonForRead(newLessonJunction);

        return Ok(lessonForReadDto);
    }

    /// <summary>
    /// Deletes a schedule lesson by Id.
    /// </summary>
    /// <response code="200">Success - Successfully deleted</response>
    [HttpDelete("{Id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<ActionResult> Delete(int scheduleLessonId)
    {
        var lesson = _context.ScheduleLessonJunctions.Find(scheduleLessonId);
        if (lesson == null)
        {
            return NotFound();
        }

        _context.ScheduleLessonJunctions.Remove(lesson);
        await _context.SaveChangesAsync();

        return Ok();
    }

    /// <summary>
    /// Deletes all lessons.
    /// </summary>
    /// <response code="200">Success - Successfully deleted</response>
    [HttpDelete]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<ActionResult> DeleteAll()
    {
        _context.ScheduleLessonJunctions.ExecuteDelete();
        await _context.SaveChangesAsync();

        return Ok();
    }

    // - - utils - -

    private async Task<ScheduleLessonJunction> CreateScheduleLessonJunction(ScheduleLessonForWriteDto lessonForWrite)
    {
        // ensure that time context exists (time/full context can't be just linked and has to be created as it's not exposed in the API)

        var timeCtxForWrite = lessonForWrite.FullContext.TimeContext;
        TimeContext timeContext = TimeContextMapper.TimeContextForWriteDtoToTimeContext(timeCtxForWrite);

        // gigantic comparator :o (perhaps I should implement .Compare on TimeContext)
        timeContext = Utls.EnsureExists(
            _context.TimeContexts, timeContext,
            (ctx) => ctx.LessonIndex == timeCtxForWrite.LessonIndex
                &&
                ctx.WeekIndex == timeCtxForWrite.WeekIndex
                &&
                ctx.WeekDay == timeCtxForWrite.WeekDay
                );

        await _context.SaveChangesAsync(); // for timeContext to get own id

        // ensure that full context exists
        var fullContext = _context.FullContexts.CreateProxy((p) =>
        {
            p.StudentsGroupId = lessonForWrite.FullContext.StudentsGroupId;
            p.TimeContextId = timeContext.Id;
        });

        fullContext = Utls.EnsureExists(
            _context.FullContexts, fullContext,
            (ctx) => ctx.StudentsGroupId == fullContext.StudentsGroupId && ctx.TimeContextId == fullContext.TimeContextId
                );

        await _context.SaveChangesAsync(); // for full context to get own id

        var newLessonJunction = _context.ScheduleLessonJunctions.CreateProxy((newLessonJunctionProxy) =>
        {
            newLessonJunctionProxy.LessonId = lessonForWrite.LessonId;
            newLessonJunctionProxy.FullContextId = fullContext.Id;
        });

        return newLessonJunction;
    }
}