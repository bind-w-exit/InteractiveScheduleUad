using InteractiveScheduleUad.Api.Models;
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
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<StudentsGroupForReadDto>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IEnumerable<StudentsGroupForReadDto>>> Get()
    {
        IEnumerable<StudentsGroupForReadDto> studentsGroups = await _studentsGroupService.GetAllAsync();

        return Ok(studentsGroups);
    }

    // GET api/<StudentsGroupController>/5
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(StudentsGroupForReadDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<StudentsGroupForReadDto>> Get(int id)
    {
        StudentsGroupForReadDto? studentsGroup = await _studentsGroupService.GetByIdAsync(id);

        if (studentsGroup is null)
            return BadRequest("Students group not found.");
        else
            return Ok(studentsGroup);
    }

    // POST api/<StudentsGroupController>
    [HttpPost]
    [ProducesResponseType(typeof(StudentsGroup), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<StudentsGroup>> Post([FromBody] string value)
    {
        StudentsGroup studentsGroup = await _studentsGroupService.CreateAsync(value);
        return Ok(studentsGroup);
    }

    // PUT api/<StudentsGroupController>/5
    [HttpPut("{id}")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult> Put(int id, [FromBody] string value)
    {
        var success = await _studentsGroupService.UpdateAsync(id, value);
        if (!success)
            return BadRequest();
        else
            return Ok();
    }

    // DELETE api/<StudentsGroupController>/5
    [HttpDelete("{id}")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult> Delete(int id)
    {
        var success = await _studentsGroupService.DeleteAsync(id);
        if (!success)
            return BadRequest();
        else
            return Ok();
    }
}