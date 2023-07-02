using InteractiveScheduleUad.Api.Models;

namespace InteractiveScheduleUad.Api.Services.Contracts;

public interface IRoomService
{
    Task<Room> CreateAsync(string name);

    Task<IEnumerable<Room>> GetAllAsync();

    Task<Room?> GetByIdAsync(int id);

    Task<bool> UpdateAsync(int id, string newName);

    Task<bool> DeleteAsync(int id);
}