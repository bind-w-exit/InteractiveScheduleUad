using InteractiveScheduleUad.Api.Models;
using InteractiveScheduleUad.Api.Models.Dtos;
using InteractiveScheduleUad.Api.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace InteractiveScheduleUad.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DepartmentController : ControllerBase
{
    private readonly IDepartmentService _departmentService;

    public DepartmentController(IDepartmentService departmentService)
    {
        _departmentService = departmentService;
    }

    // GET: api/<DepartmentController>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Department>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IEnumerable<Department>>> Get()
    {
        var departments = await _departmentService.GetAllAsync();

        return Ok(departments);
    }

    // GET api/<DepartmentController>/5
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Department), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<Department>> Get(int id)
    {
        var department = await _departmentService.GetByIdAsync(id);

        if (department is null)
            return BadRequest("Department group not found.");
        else
            return Ok(department);
    }

    // POST api/<DepartmentController>
    [HttpPost]
    [ProducesResponseType(typeof(Department), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<Department>> Post([FromBody] DepartmentForWriteDto value)
    {
        var departmentFromDb = await _departmentService.CreateAsync(value);
        return Ok(departmentFromDb);
    }

    // PUT api/<DepartmentController>/5
    [HttpPut("{id}")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult> Put(int id, [FromBody] DepartmentForWriteDto value)
    {
        var success = await _departmentService.UpdateAsync(id, value);
        if (!success)
            return BadRequest();
        else
            return Ok();
    }

    // DELETE api/<DepartmentController>/5
    [HttpDelete("{id}")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult> Delete(int id)
    {
        var success = await _departmentService.DeleteAsync(id);
        if (!success)
            return BadRequest();
        else
            return Ok();
    }
}