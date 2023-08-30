using InteractiveScheduleUad.Api.Models;
using InteractiveScheduleUad.Api.Models.Dtos;

namespace InteractiveScheduleUad.Api.Services.Contracts;

public interface IRoomService
{
    Task<FluentResults.Result<Room>> CreateAsync(RoomForWriteDto roomForWriteDto);

    Task<IEnumerable<Room>> GetAllAsync();

    Task<Room?> GetByIdAsync(int id);

    Task<bool> UpdateAsync(int id, RoomForWriteDto roomForWriteDto);

    Task<bool> DeleteAsync(int id);
}