using InteractiveScheduleUad.Api.Models;

namespace InteractiveScheduleUad.Api.Services.Contracts;

public interface ICourseService
{
    Task<Course> CreateAsync(string name);

    Task<IEnumerable<Course>> GetAllAsync();

    Task<Course?> GetByIdAsync(int id);

    Task<bool> UpdateAsync(int id, string newName);

    Task<bool> DeleteAsync(int id);
}