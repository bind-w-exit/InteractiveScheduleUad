using InteractiveScheduleUad.Api.Models;
using InteractiveScheduleUad.Api.Models.Dtos;

namespace InteractiveScheduleUad.Api.Services.Contracts;

public interface IStudentsGroupService
{
    Task<StudentsGroupForReadDto> CreateAsync(string name);

    Task<IEnumerable<StudentsGroupForReadDto>> GetAllAsync();

    Task<StudentsGroupForReadDto?> GetByIdAsync(int id);

    Task<bool> UpdateAsync(int id, string newName);

    Task<bool> DeleteAsync(int id);
}