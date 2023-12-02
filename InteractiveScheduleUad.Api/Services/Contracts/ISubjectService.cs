using FluentResults;
using InteractiveScheduleUad.Api.Models;

namespace InteractiveScheduleUad.Api.Services.Contracts;

public interface ISubjectService
{
    Task<Result<Subject>> CreateAsync(Subject subject);

    Task<Result<IEnumerable<Subject>>> GetAllAsync();

    Task<Result<Subject>> GetByIdAsync(int id);

    Task<Result> UpdateAsync(int id, string newName);

    Task<Result> DeleteAsync(int id);
}