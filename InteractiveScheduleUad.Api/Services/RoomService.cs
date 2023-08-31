using FluentResults;
using FluentValidation;
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

    public async Task<bool> DeleteAsync(int id)
    {
        var room = await _roomRepository.GetByIdAsync(id);
        if (room is not null)
        {
            _roomRepository.Delete(room);
            await _roomRepository.SaveChangesAsync();
            return true;
        }
        return false;
    }

    public async Task<IEnumerable<Room>> GetAllAsync()
    {
        return await _roomRepository.GetAllAsync();
    }

    public async Task<Room?> GetByIdAsync(int id)
    {
        return await _roomRepository.GetByIdAsync(id);
    }

    public async Task<bool> UpdateAsync(int id, RoomForWriteDto roomForWriteDto)
    {
        var roomFromDb = await _roomRepository.GetByIdAsync(id);
        if (roomFromDb is not null)
        {
            roomFromDb.Name = roomForWriteDto.Name;

            _roomRepository.Update(roomFromDb);
            await _roomRepository.SaveChangesAsync();
            return true;
        }
        return false;
    }
}