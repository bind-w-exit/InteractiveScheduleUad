using InteractiveScheduleUad.Api.Models;
using InteractiveScheduleUad.Api.Models.Dtos;
using InteractiveScheduleUad.Api.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace InteractiveScheduleUad.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TeacherController : ControllerBase
{
    private readonly ITeacherService _teacherService;

    public TeacherController(ITeacherService teacherService)
    {
        _teacherService = teacherService;
    }

    // GET: api/<TeacherController>
    /// <summary>
    /// Retrieves all teachers
    /// </summary>
    /// <response code="200">Success - Returns an array of teachers</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Teacher>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IEnumerable<Teacher>>> Get()
    {
        IEnumerable<Teacher> teachers = await _teacherService.GetAllAsync();

        return Ok(teachers);
    }

    // GET api/<TeacherController>/5
    /// <summary>
    /// Retrieves a teacher by its ID
    /// </summary>
    /// <param name="id">Teacher ID</param>
    /// <response code="200">Success - Returns the teacher with the specified ID</response>
    /// <response code="404">NotFound - Teacher with the specified ID was not found</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Teacher), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public async Task<ActionResult<Teacher>> Get(int id)
    {
        Teacher? teacher = await _teacherService.GetByIdAsync(id);

        if (teacher is null)
            return NotFound("Teacher with the specified ID was not found");
        else
            return Ok(teacher);
    }

    // POST api/<TeacherController>
    /// <summary>
    /// Creates a new teacher
    /// </summary>
    /// <param name="teacher">The teacher data</param>
    /// <response code="201">Created - Returns the created teacher</response>
    /// <response code="400">BadRequest - One or more validation errors occurred</response>
    [HttpPost]
    [ProducesResponseType(typeof(Teacher), (int)HttpStatusCode.Created)]
    public async Task<ActionResult<Teacher>> Post([FromBody] TeacherForWriteDto teacher)
    {
        Teacher teacherFromDb = await _teacherService.CreateAsync(teacher);

        return CreatedAtAction(nameof(Get), new { id = teacherFromDb.Id }, teacherFromDb);
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
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public async Task<ActionResult> Put(int id, [FromBody] TeacherForWriteDto newTeacher)
    {
        bool success = await _teacherService.UpdateAsync(id, newTeacher);

        if (!success)
            return NotFound("Teacher with the specified ID was not found");
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
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public async Task<ActionResult> Delete(int id)
    {
        bool success = await _teacherService.DeleteAsync(id);

        if (!success)
            return NotFound("Teacher with the specified ID was not found");
        else
            return Ok();
    }
}