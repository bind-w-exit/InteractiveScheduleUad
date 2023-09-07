using FluentResults;
using InteractiveScheduleUad.Api.Models;
using InteractiveScheduleUad.Api.Models.Dtos;

namespace InteractiveScheduleUad.Api.Services.Contracts;

public interface IRoomService
{
    Task<Result<Room>> CreateAsync(RoomForWriteDto roomForWriteDto);

    Task<Result<IEnumerable<Room>>> GetAllAsync();

    Task<Result<Room>> GetByIdAsync(int id);

    Task<Result> UpdateAsync(int id, RoomForWriteDto roomForWriteDto);

    Task<Result> DeleteAsync(int id);
}