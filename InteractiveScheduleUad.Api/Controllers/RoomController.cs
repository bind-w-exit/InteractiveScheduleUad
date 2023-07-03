using InteractiveScheduleUad.Api.Models;
using InteractiveScheduleUad.Api.Services.Contracts;
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
    [ProducesResponseType(typeof(IEnumerable<Room>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IEnumerable<Room>>> Get()
    {
        IEnumerable<Room> rooms = await _roomService.GetAllAsync();
        return Ok(rooms);
    }

    // GET api/<RoomController>/5
    /// <summary>
    /// Retrieves a room by its ID
    /// </summary>
    /// <param name="id">Room ID</param>
    /// <response code="200">Success - Returns the room with the specified ID</response>
    /// <response code="404">NotFound - Room with the specified ID was not found</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Room), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public async Task<ActionResult<Room>> Get(int id)
    {
        Room? room = await _roomService.GetByIdAsync(id);

        if (room is null)
            return NotFound("Room with the specified ID was not found");
        else
            return Ok(room);
    }

    // POST api/<RoomController>
    /// <summary>
    /// Creates a new room
    /// </summary>
    /// <param name="newRoom">The room data</param>
    /// <response code="201">Created - Returns the created room</response>
    /// <response code="400">One or more validation errors occurred</response>
    [HttpPost]
    [ProducesResponseType(typeof(Room), (int)HttpStatusCode.Created)]
    public async Task<ActionResult<Room>> Post([FromBody] string newRoom)
    {
        Room room = await _roomService.CreateAsync(newRoom);
        return CreatedAtAction(nameof(Get), new { id = room.Id }, room);
    }

    // PUT api/<RoomController>/5
    /// <summary>
    /// Updates an existing room
    /// </summary>
    /// <param name="id">The ID of the room to update</param>
    /// <param name="newRoom">The updated room data</param>
    /// <response code="200">Success - Successfully updated</response>
    /// <response code="400">One or more validation errors occurred</response>
    /// <response code="404">NotFound - Room with the specified ID was not found</response>
    [HttpPut("{id}")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public async Task<ActionResult> Put(int id, [FromBody] string newRoom)
    {
        var success = await _roomService.UpdateAsync(id, newRoom);
        if (!success)
            return NotFound("Room with the specified ID was not found");
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
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public async Task<ActionResult> Delete(int id)
    {
        var success = await _roomService.DeleteAsync(id);
        if (!success)
            return NotFound("Room with the specified ID was not found");
        else
            return Ok();
    }
}