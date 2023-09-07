using FluentResults;
using FluentValidation;
using InteractiveScheduleUad.Api.Errors;
using InteractiveScheduleUad.Api.Extensions;
using InteractiveScheduleUad.Api.Models;
using InteractiveScheduleUad.Api.Models.Dtos;
using InteractiveScheduleUad.Api.Repositories.Contracts;
using InteractiveScheduleUad.Api.Services.Contracts;

namespace InteractiveScheduleUad.Api.Services;

public class RoomService : IRoomService
{
    private readonly IRoomRepository _roomRepository;
    private readonly IValidator<RoomForWriteDto> _roomValidator;

    public RoomService(IRoomRepository roomRepository, IValidator<RoomForWriteDto> roomValidator)
    {
        _roomRepository = roomRepository;
        _roomValidator = roomValidator;
    }

    public async Task<Result<Room>> CreateAsync(RoomForWriteDto roomForWriteDto)
    {
        var validationResult = _roomValidator.Validate(roomForWriteDto);

        if (!validationResult.IsValid)
        {
            return validationResult.Errors.ToValidationError();
        }

        Room room = new() { Name = roomForWriteDto.Name };

        await _roomRepository.InsertAsync(room);
        await _roomRepository.SaveChangesAsync();

        return room;
    }

    public async Task<Result> DeleteAsync(int id)
    {
        var room = await _roomRepository.GetByIdAsync(id);

        if (room is not null)
        {
            _roomRepository.Delete(room);
            await _roomRepository.SaveChangesAsync();

            return Result.Ok();
        }
        else
            return new NotFoundError(nameof(Room));
    }

    public async Task<Result<IEnumerable<Room>>> GetAllAsync()
    {
        var rooms = await _roomRepository.GetAllAsync();

        return Result.Ok(rooms);
    }

    public async Task<Result<Room>> GetByIdAsync(int id)
    {
        var room = await _roomRepository.GetByIdAsync(id);

        if (room is not null)
            return room;
        else
            return new NotFoundError(nameof(Room));
    }

    public async Task<Result> UpdateAsync(int id, RoomForWriteDto roomForWriteDto)
    {
        var validationResult = _roomValidator.Validate(roomForWriteDto);

        if (!validationResult.IsValid)
        {
            return validationResult.Errors.ToValidationError();
        }

        var room = await _roomRepository.GetByIdAsync(id);

        if (room is not null)
        {
            room.Name = roomForWriteDto.Name;

            _roomRepository.Update(room);
            await _roomRepository.SaveChangesAsync();

            return Result.Ok();
        }
        else
            return new NotFoundError(nameof(Room));
    }
}