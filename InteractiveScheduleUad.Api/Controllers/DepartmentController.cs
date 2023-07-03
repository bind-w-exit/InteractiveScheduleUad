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
    /// <summary>
    /// Returns all departments
    /// </summary>
    /// <response code="200">Success - Returns an array of departments</response>
    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(typeof(IEnumerable<Department>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IEnumerable<Department>>> Get()
    {
        var departments = await _departmentService.GetAllAsync();

        return Ok(departments);
    }

    // GET api/<DepartmentController>/5
    /// <summary>
    /// Retrieves a department by its ID
    /// </summary>
    /// <param name="id">Department ID</param>
    /// <response code="200">Success - Returns the department with the specified ID</response>
    /// <response code="404">Not Found - Department with the specified ID was not found</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Department), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public async Task<ActionResult<Department>> Get(int id)
    {
        var department = await _departmentService.GetByIdAsync(id);

        if (department is null)
            return NotFound("Department group not found.");
        else
            return Ok(department);
    }

    // POST api/<DepartmentController>
    /// <summary>
    /// Creates a new department
    /// </summary>
    /// <param name="newDepartment">The department data</param>
    /// <response code="201">Success - Returns the created department</response>
    /// <response code="400">One or more validation errors occurred</response>
    [HttpPost]
    [ProducesResponseType(typeof(Department), (int)HttpStatusCode.Created)]
    public async Task<ActionResult<Department>> Post([FromBody] DepartmentForWriteDto newDepartment)
    {
        var departmentFromDb = await _departmentService.CreateAsync(newDepartment);
        return Ok(departmentFromDb);
    }

    // PUT api/<DepartmentController>/5
    /// <summary>
    /// Updates an existing department
    /// </summary>
    /// <param name="id">The ID of the department to update</param>
    /// <param name="newDepartment">The updated department data</param>
    /// <response code="200">Success - Successfully updated</response>
    /// <response code="400">One or more validation errors occurred</response>
    /// <response code="404">NotFound - Department with the specified ID was not found</response>
    [HttpPut("{id}")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public async Task<ActionResult> Put(int id, [FromBody] DepartmentForWriteDto newDepartment)
    {
        var success = await _departmentService.UpdateAsync(id, newDepartment);
        if (!success)
            return NotFound("Department with the specified ID was not found");
        else
            return Ok();
    }

    // DELETE api/<DepartmentController>/5
    /// <summary>
    /// Deletes a department
    /// </summary>
    /// <param name="id">The ID of the department to delete</param>
    /// <response code="200">Success - Successfully deleted</response>
    /// <response code="404">NotFound - Department with the specified ID was not found</response>
    [HttpDelete("{id}")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public async Task<ActionResult> Delete(int id)
    {
        var success = await _departmentService.DeleteAsync(id);
        if (!success)
            return NotFound("Department with the specified ID was not found");
        else
            return Ok();
    }
}