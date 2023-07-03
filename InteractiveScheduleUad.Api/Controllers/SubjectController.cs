using InteractiveScheduleUad.Api.Models;
using InteractiveScheduleUad.Api.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace InteractiveScheduleUad.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SubjectController : ControllerBase
{
    private readonly ISubjectService _subjectService;

    public SubjectController(ISubjectService subjectService)
    {
        _subjectService = subjectService;
    }

    // GET: api/<SubjectController>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Subject>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IEnumerable<Subject>>> Get()
    {
        IEnumerable<Subject> subjects = await _subjectService.GetAllAsync();

        return Ok(subjects);
    }

    // GET api/<SubjectController>/5
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Subject), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<Subject>> Get(int id)
    {
        Subject? subject = await _subjectService.GetByIdAsync(id);

        if (subject is null)
            return BadRequest("Subject not found.");
        else
            return Ok(subject);
    }

    // POST api/<SubjectController>
    [HttpPost]
    [ProducesResponseType(typeof(Subject), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<Subject>> Post([FromBody] string value)
    {
        Subject subject = await _subjectService.CreateAsync(value);
        return Ok(subject);
    }

    // PUT api/<SubjectController>/5
    [HttpPut("{id}")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult> Put(int id, [FromBody] string value)
    {
        var success = await _subjectService.UpdateAsync(id, value);
        if (!success)
            return BadRequest();
        else
            return Ok();
    }

    // DELETE api/<SubjectController>/5
    [HttpDelete("{id}")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult> Delete(int id)
    {
        var success = await _subjectService.DeleteAsync(id);
        if (!success)
            return BadRequest();
        else
            return Ok();
    }
}