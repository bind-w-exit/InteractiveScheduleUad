using FluentResults;
using InteractiveScheduleUad.Api.Models;
using InteractiveScheduleUad.Api.Models.Dtos;

namespace InteractiveScheduleUad.Api.Services.Contracts;

public interface IDepartmentService
{
    Task<Result<Department>> CreateAsync(DepartmentForWriteDto departmentCreateDto);

    Task<Result<IEnumerable<Department>>> GetAllAsync();

    Task<Result<Department>> GetByIdAsync(int id);

    Task<Result> UpdateAsync(int id, DepartmentForWriteDto departmentCreateDto);

    Task<Result> DeleteAsync(int id);
}