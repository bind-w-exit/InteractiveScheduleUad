using InteractiveScheduleUad.Api.Extensions;
using InteractiveScheduleUad.Api.Models;
using InteractiveScheduleUad.Api.Models.Dtos;
using InteractiveScheduleUad.Api.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace InteractiveScheduleUad.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RoomController : ControllerBase
{
    private readonly IRoomService _roomService;

    public RoomController(IRoomService roomService)
    {
        _roomService = roomService;
    }

    // GET: api/<RoomController>
    /// <summary>
    /// Retrieves all rooms
    /// </summary>
    /// <response code="200">Success - Returns an array of rooms</response>
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(IEnumerable<Room>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IEnumerable<Room>>> Get()
    {
        var result = await _roomService.GetAllAsync();

        if (result.IsFailed)
            return result.Errors.First().ToObjectResult();
        else
            return Ok(result.Value);
    }

    // GET api/<RoomController>/5
    /// <summary>
    /// Retrieves a room by its ID
    /// </summary>
    /// <param name="id">Room ID</param>
    /// <response code="200">Success - Returns the room with the specified ID</response>
    /// <response code="404">NotFound - Room with the specified ID was not found</response>
    [HttpGet("{id}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(Room), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public async Task<ActionResult<Room>> Get(int id)
    {
        var result = await _roomService.GetByIdAsync(id);

        if (result.IsFailed)
            return result.Errors.First().ToObjectResult();
        else
            return Ok(result.Value);
    }

    // POST api/<RoomController>
    /// <summary>
    /// Creates a new room
    /// </summary>
    /// <param name="roomForWriteDto">The room name</param>
    /// <response code="201">Created - Returns the created room</response>
    /// <response code="400">BadRequest - One or more validation errors occurred</response>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(Room), (int)HttpStatusCode.Created)]
    public async Task<ActionResult<Room>> Post([FromBody] RoomForWriteDto roomForWriteDto)
    {
        var result = await _roomService.CreateAsync(roomForWriteDto);

        if (result.IsFailed)
            return result.Errors.First().ToObjectResult();

        var createdRoom = result.Value;
        return CreatedAtAction(nameof(Get), new { id = createdRoom.Id }, createdRoom);
    }

    // PUT api/<RoomController>/5
    /// <summary>
    /// Updates an existing room
    /// </summary>
    /// <param name="id">The ID of the room to update</param>
    /// <param name="roomForWriteDto">The updated room name</param>
    /// <response code="200">Success - Successfully updated</response>
    /// <response code="400">BadRequest - One or more validation errors occurred</response>
    /// <response code="404">NotFound - Room with the specified ID was not found</response>
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public async Task<ActionResult> Put(int id, [FromBody] RoomForWriteDto roomForWriteDto)
    {
        var result = await _roomService.UpdateAsync(id, roomForWriteDto);

        if (result.IsFailed)
            return result.Errors.First().ToObjectResult();
        else
            return Ok();
    }

    // DELETE api/<RoomController>/5
    /// <summary>
    /// Deletes a room
    /// </summary>
    /// <param name="id">The ID of the room to delete</param>
    /// <response code="200">Success - Successfully deleted</response>
    /// <response code="404">NotFound - Room with the specified ID was not found</response>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public async Task<ActionResult> Delete(int id)
    {
        var result = await _roomService.DeleteAsync(id);

        if (result.IsFailed)
            return result.Errors.First().ToObjectResult();
        else
            return Ok();
    }
}