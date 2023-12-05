using InteractiveScheduleUad.Api.Controllers.Contracts;
using InteractiveScheduleUad.Api.Extensions;
using InteractiveScheduleUad.Api.Models;
using InteractiveScheduleUad.Api.Models.Dtos;
using InteractiveScheduleUad.Api.Models.Filters;
using InteractiveScheduleUad.Api.Services.Contracts;
using InteractiveScheduleUad.Api.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace InteractiveScheduleUad.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TeacherDepartmentController : ControllerBase, IReactAdminCompatible<TeacherDepartmentForReadDto>
{
    private readonly IDepartmentService _departmentService;
    private readonly InteractiveScheduleUadApiDbContext _context;

    public TeacherDepartmentController(IDepartmentService departmentService, InteractiveScheduleUadApiDbContext context)
    {
        _departmentService = departmentService;
        _context = context;
    }

    // GET: api/<DepartmentController>
    /// <summary>
    /// Retrieves all departments
    /// </summary>
    /// <response code="200">Success - Returns an array of departments</response>
    //[HttpGet]
    //[AllowAnonymous]
    //[Produces("application/json")]
    //[ProducesResponseType(typeof(IEnumerable<Department>), (int)HttpStatusCode.OK)]
    //public async Task<ActionResult<IEnumerable<Department>>> Get()
    //{
    //    var result = await _departmentService.GetAllAsync();

    //    if (result.IsFailed)
    //        return result.Errors.First().ToObjectResult();
    //    else
    //        return Ok(result.Value);
    //}

    // GET: api/<DepartmentController>
    /// <summary>
    /// Retrieves a list of departments
    /// </summary>
    /// <response code="200">Success - Returns a list of departments</response>
    [HttpGet]
    [AllowAnonymous]
    [Produces("application/json")]
    [ProducesResponseType(typeof(IEnumerable<Department>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IEnumerable<TeacherDepartmentForReadDto>>> GetList(
        [FromQuery] string range = "[0, 999999]",
        [FromQuery] string sort = "[\"Id\", \"ASC\"]",
        [FromQuery] string filter = "{}")
    {
        var resultsRange = Utls
            .FilterSortAndRangeDbSet<Department, TeacherDepartmentForReadDtoFilter>(
            _context,
            range, sort, filter,
            out int rangeStart, out int rangeEnd);

        var totalCount = _context.Departments.Count();
        Utls.AddContentRangeHeader(
            rangeStart, rangeEnd, totalCount,
            ControllerContext, Response);

        //var resultsForRead = resultsRange.Select(DepartmentMapper.DepartmentToTeacherDepartmentForReadDto);
        var resultsForRead = resultsRange;

        return Ok(resultsForRead);
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
    public async Task<ActionResult<Department>> Post([FromBody] TeacherDepartmentForWriteDto department)
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
    public async Task<ActionResult> Put(int id, [FromBody] TeacherDepartmentForWriteDto newDepartment)
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

    /// <summary>
    /// Deletes all department
    /// </summary>
    /// <response code="200">Success - Successfully deleted</response>
    [HttpDelete]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<ActionResult> DeleteAll()
    {
        await _departmentService.DeleteAll();

        return Ok();
        //if (result.IsFailed)
        //    return result.Errors.First().ToObjectResult();
        //else
        //    return Ok();
    }
}