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
using SQLitePCL;
using System.Net;

namespace InteractiveScheduleUad.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TeacherController : ControllerBase, IReactAdminCompatible<TeacherForReadDto>
{
    private readonly ITeacherService _teacherService;
    private InteractiveScheduleUadApiDbContext _context;

    public TeacherController(ITeacherService teacherService, InteractiveScheduleUadApiDbContext dbContext)
    {
        _teacherService = teacherService;
        _context = dbContext;
    }

    // GET: api/<TeacherController>
    /// <summary>
    /// Retrieves a list of teachers.
    /// </summary>
    /// <response code="200">Success - Returns an array of teachers</response>
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(IEnumerable<TeacherForReadDto>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IEnumerable<TeacherForReadDto>>> GetList(
        [FromQuery] string range = "[0, 999999]",
        [FromQuery] string sort = "[\"Id\", \"ASC\"]",
        [FromQuery] string filter = "{}")
    {
        var resultsRange = Utls
           .FilterSortAndRangeDbSet<Teacher, TeacherForReadDtoFilter>(
            _context,
            range, sort, filter,
            out int rangeStart, out int rangeEnd);

        var totalCount = _context.Teachers.Count();
        Utls.AddContentRangeHeader(
            rangeStart, rangeEnd, totalCount,
            ControllerContext, Response);

        var resultsForRead = resultsRange.Select(TeacherMapper.TeacherToTeacherForReadDto);

        return Ok(resultsForRead);
    }

    // GET api/<TeacherController>/5
    /// <summary>
    /// Retrieves a teacher by its ID
    /// </summary>
    /// <param name="id">Teacher ID</param>
    /// <response code="200">Success - Returns the teacher with the specified ID</response>
    /// <response code="404">NotFound - Teacher with the specified ID was not found</response>
    [HttpGet("{id}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(Teacher), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public async Task<ActionResult<Teacher>> Get(int id)
    {
        var result = await _teacherService.GetByIdAsync(id);

        if (result.IsFailed)
            return result.Errors.First().ToObjectResult();
        else
            return Ok(result.Value);
    }

    // POST api/<TeacherController>
    /// <summary>
    /// Creates a new teacher
    /// </summary>
    /// <param name="teacher">The teacher data</param>
    /// <response code="201">Created - Returns the created teacher</response>
    [HttpPost]
    //[Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(Teacher), (int)HttpStatusCode.Created)]
    public async Task<ActionResult<Teacher>> Post([FromBody] TeacherForWriteDto teacher)
    {
        //var result = await _teacherService.CreateAsync(teacher);

        var newTeacherProxy = _context.Teachers.CreateProxy();
        TeacherMapper.TeacherForWriteDtoToTeacher(teacher, newTeacherProxy);

        _context.Add(newTeacherProxy);
        _context.SaveChanges();

        return Ok(newTeacherProxy);

        //var teacherForRead = await _context.Teachers.AddAsync(newTeacherProxy);

        //if (result.IsFailed)
        //    return result.Errors.First().ToObjectResult();

        //var createdTeacher = await _teacherService.GetByIdAsync(result.Value.Id);
        //return CreatedAtAction(nameof(Get), new { id = createdTeacher.Id }, createdTeacher);
    }

    // PUT api/<TeacherController>/5
    /// <summary>
    /// Updates an existing teacher
    /// </summary>
    /// <param name="id">The ID of the teacher to update</param>
    /// <param name="newTeacher">The updated teacher data</param>
    /// <response code="200">Success - Successfully updated</response>
    /// <response code="400">BadRequest - One or more validation errors occurred</response>
    /// <response code="404">NotFound - Teacher with the specified ID was not found</response>
    [HttpPut("{id}")]
    //[Authorize(Roles = "Admin")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public async Task<ActionResult> Put(int id, [FromBody] TeacherForWriteDto newTeacher)
    {
        var result = await _teacherService.UpdateAsync(id, newTeacher);

        if (result.IsFailed)
            return result.Errors.First().ToObjectResult();
        else
            return Ok();
    }

    // DELETE api/<TeacherController>/5
    /// <summary>
    /// Deletes a teacher
    /// </summary>
    /// <param name="id">The ID of the teacher to delete</param>
    /// <response code="200">Success - Successfully deleted</response>
    /// <response code="404">NotFound - Teacher with the specified ID was not found</response>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public async Task<ActionResult> Delete(int id)
    {
        var result = await _teacherService.DeleteAsync(id);

        if (result.IsFailed)
            return result.Errors.First().ToObjectResult();
        else
            return Ok();
    }

    // DELETE api/<TeacherController>/5
    /// <summary>
    /// Deletes all teachers
    /// </summary>
    /// <response code="200">Success - Successfully deleted</response>
    [HttpDelete]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public async Task<ActionResult> DeleteAll()
    {
        //await _context.Teachers.ExecuteDeleteAsync();
        _context.Teachers.RemoveRange(_context.Teachers);
        await _context.SaveChangesAsync();

        return Ok();

        //if (result.IsFailed)
        //    return result.Errors.First().ToObjectResult();
        //else
        //    return Ok();
    }
}