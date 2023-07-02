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
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Room>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IEnumerable<Room>>> Get()
    {
        IEnumerable<Room> rooms = await _roomService.GetAllAsync();

        return Ok(rooms);
    }

    // GET api/<RoomController>/5
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Room), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<Room>> Get(int id)
    {
        Room? room = await _roomService.GetByIdAsync(id);

        if (room is null)
            return BadRequest("Room not found.");
        else
            return Ok(room);
    }

    // POST api/<RoomController>
    [HttpPost]
    [ProducesResponseType(typeof(Room), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<Room>> Post([FromBody] string value)
    {
        Room room = await _roomService.CreateAsync(value);
        return Ok(room);
    }

    // PUT api/<RoomController>/5
    [HttpPut("{id}")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult> Put(int id, [FromBody] string value)
    {
        var success = await _roomService.UpdateAsync(id, value);
        if (!success)
            return BadRequest();
        else
            return Ok();
    }

    // DELETE api/<RoomController>/5
    [HttpDelete("{id}")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult> Delete(int id)
    {
        var success = await _roomService.DeleteAsync(id);
        if (!success)
            return BadRequest();
        else
            return Ok();
    }
}