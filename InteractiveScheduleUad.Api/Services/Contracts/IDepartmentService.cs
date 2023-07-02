using InteractiveScheduleUad.Api.Models;
using InteractiveScheduleUad.Api.Models.Dtos;

namespace InteractiveScheduleUad.Api.Services.Contracts;

public interface IDepartmentService
{
    Task<Department> CreateAsync(DepartmentForWriteDto departmentCreateDto);

    Task<IEnumerable<Department>> GetAllAsync();

    Task<Department?> GetByIdAsync(int id);

    Task<bool> UpdateAsync(int id, DepartmentForWriteDto departmentCreateDto);

    Task<bool> DeleteAsync(int id);
}