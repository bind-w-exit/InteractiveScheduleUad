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
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Teacher>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IEnumerable<Teacher>>> Get()
    {
        var teachers = await _teacherService.GetAllAsync();

        return Ok(teachers);
    }

    // GET api/<TeacherController>/5
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Teacher), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<Teacher>> Get(int id)
    {
        var teacher = await _teacherService.GetByIdAsync(id);

        if (teacher is null)
            return BadRequest("Teacher not found.");
        else
            return Ok(teacher);
    }

    // POST api/<TeacherController>
    [HttpPost]
    [ProducesResponseType(typeof(Teacher), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<Teacher>> Post([FromBody] TeacherForWriteDto value)
    {
        var teacherFromDb = await _teacherService.CreateAsync(value);
        return Ok(teacherFromDb);
    }

    // PUT api/<TeacherController>/5
    [HttpPut("{id}")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult> Put(int id, [FromBody] TeacherForWriteDto value)
    {
        var success = await _teacherService.UpdateAsync(id, value);
        if (!success)
            return BadRequest();
        else
            return Ok();
    }

    // DELETE api/<TeacherController>/5
    [HttpDelete("{id}")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult> Delete(int id)
    {
        var success = await _teacherService.DeleteAsync(id);
        if (!success)
            return BadRequest();
        else
            return Ok();
    }
}