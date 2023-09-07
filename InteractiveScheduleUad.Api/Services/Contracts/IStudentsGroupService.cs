using FluentResults;
using InteractiveScheduleUad.Api.Models.Dtos;

namespace InteractiveScheduleUad.Api.Services.Contracts;

public interface IStudentsGroupService
{
    Task<Result<StudentsGroupForReadDto>> CreateAsync(string name);

    Task<Result<IEnumerable<StudentsGroupForReadDto>>> GetAllAsync();

    Task<Result<StudentsGroupForReadDto>> GetByIdAsync(int id);

    Task<Result> UpdateAsync(int id, string newName);

    Task<Result> DeleteAsync(int id);
}