using InteractiveScheduleUad.Api.Models;
using InteractiveScheduleUad.Api.Repositories.Contracts;
using InteractiveScheduleUad.Api.Services.Contracts;

namespace InteractiveScheduleUad.Api.Services;

public class RoomService : IRoomService
{
    private readonly IRoomRepository _roomRepository;

    public RoomService(IRoomRepository roomRepository)
    {
        _roomRepository = roomRepository;
    }

    public async Task<Room> CreateAsync(string name)
    {
        Room room = new() { Name = name };

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

    public async Task<bool> UpdateAsync(int id, string newName)
    {
        var roomFromDb = await _roomRepository.GetByIdAsync(id);
        if (roomFromDb is not null)
        {
            roomFromDb.Name = newName;

            _roomRepository.Update(roomFromDb);
            await _roomRepository.SaveChangesAsync();
            return true;
        }
        return false;
    }
}