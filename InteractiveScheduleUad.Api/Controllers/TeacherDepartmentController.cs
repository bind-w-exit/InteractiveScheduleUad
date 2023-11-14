﻿using InteractiveScheduleUad.Api.Extensions;
using InteractiveScheduleUad.Api.Models;
using InteractiveScheduleUad.Api.Models.Dtos;
using InteractiveScheduleUad.Api.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace InteractiveScheduleUad.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TeacherDepartmentController : ControllerBase
{
    private readonly IDepartmentService _departmentService;

    public TeacherDepartmentController(IDepartmentService departmentService)
    {
        _departmentService = departmentService;
    }

    // GET: api/<DepartmentController>
    /// <summary>
    /// Retrieves all departments
    /// </summary>
    /// <response code="200">Success - Returns an array of departments</response>
    [HttpGet]
    [AllowAnonymous]
    [Produces("application/json")]
    [ProducesResponseType(typeof(IEnumerable<Department>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IEnumerable<Department>>> Get()
    {
        var result = await _departmentService.GetAllAsync();

        if (result.IsFailed)
            return result.Errors.First().ToObjectResult();
        else
            return Ok(result.Value);
    }

    // GET api/<DepartmentController>/5
    /// <summary>
    /// Retrieves a department by its ID
    /// </summary>
    /// <param name="id">Department ID</param>
    /// <response code="200">Success - Returns the department with the specified ID</response>
    /// <response code="404">NotFound - Department with the specified ID was not found</response>
    [HttpGet("{id}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(Department), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public async Task<ActionResult<Department>> Get(int id)
    {
        var result = await _departmentService.GetByIdAsync(id);

        if (result.IsFailed)
            return result.Errors.First().ToObjectResult();
        else
            return Ok(result.Value);
    }

    // POST api/<DepartmentController>
    /// <summary>
    /// Creates a new department
    /// </summary>
    /// <param name="department">The department data</param>
    /// <response code="201">Created - Returns the created department</response>
    /// <response code="400">BadRequest - One or more validation errors occurred</response>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(Department), (int)HttpStatusCode.Created)]
    public async Task<ActionResult<Department>> Post([FromBody] DepartmentForWriteDto department)
    {
        var result = await _departmentService.CreateAsync(department);

        if (result.IsFailed)
            return result.Errors.First().ToObjectResult();

        var createdDepartment = result.Value;
        // TODO: Annotate
        return CreatedAtAction(nameof(Get), new { id = createdDepartment.Id }, createdDepartment);
    }

    // PUT api/<DepartmentController>/5
    /// <summary>
    /// Updates an existing department
    /// </summary>
    /// <param name="id">The ID of the department to update</param>
    /// <param name="newDepartment">The updated department data</param>
    /// <response code="200">Success - Successfully updated</response>
    /// <response code="400">BadRequest - One or more validation errors occurred</response>
    /// <response code="404">NotFound - Department with the specified ID was not found</response>
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public async Task<ActionResult> Put(int id, [FromBody] DepartmentForWriteDto newDepartment)
    {
        var result = await _departmentService.UpdateAsync(id, newDepartment);

        if (result.IsFailed)
            return result.Errors.First().ToObjectResult();
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
    [Authorize(Roles = "Admin")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public async Task<ActionResult> Delete(int id)
    {
        var result = await _departmentService.DeleteAsync(id);

        if (result.IsFailed)
            return result.Errors.First().ToObjectResult();
        else
            return Ok();
    }
}