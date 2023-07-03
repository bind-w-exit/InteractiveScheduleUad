using InteractiveScheduleUad.Api.Models;

namespace InteractiveScheduleUad.Api.Services.Contracts;

public interface ISubjectService
{
    Task<Subject> CreateAsync(string name);

    Task<IEnumerable<Subject>> GetAllAsync();

    Task<Subject?> GetByIdAsync(int id);

    Task<bool> UpdateAsync(int id, string newName);

    Task<bool> DeleteAsync(int id);
}