using InteractiveScheduleUad.Api.Models;
using InteractiveScheduleUad.Api.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Net;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace InteractiveScheduleUad.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CourseController : ControllerBase
{
    private readonly ICourseService _courseService;

    public CourseController(ICourseService courseService)
    {
        _courseService = courseService;
    }

    // GET: api/<CourseController>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Course>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IEnumerable<Course>>> Get()
    {
        IEnumerable<Course> courses = await _courseService.GetAllAsync();

        return Ok(courses);
    }

    // GET api/<CourseController>/5
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Course), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<Course>> Get(int id)
    {
        Course? course = await _courseService.GetByIdAsync(id);

        if (course is null)
            return BadRequest("Course not found.");
        else
            return Ok(course);
    }

    [HttpPost]
    [ProducesResponseType(typeof(Course), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<Course>> Post([FromBody] string value)
    {
        Course course = await _courseService.CreateAsync(value);
        return Ok(course);
    }

    // PUT api/<CourseController>/5
    [HttpPut("{id}")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult> Put(int id, [FromBody] string value)
    {
        var success = await _courseService.UpdateAsync(id, value);
        if (!success)
            return BadRequest();
        else
            return Ok();
    }

    // DELETE api/<CourseController>/5
    [HttpDelete("{id}")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult> Delete(int id)
    {
        var success = await _courseService.DeleteAsync(id);
        if (!success)
            return BadRequest();
        else
            return Ok();
    }
}