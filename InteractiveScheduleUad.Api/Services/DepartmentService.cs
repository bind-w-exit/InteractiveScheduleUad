using FluentResults;
using InteractiveScheduleUad.Api.Errors;
using InteractiveScheduleUad.Api.Mappers;
using InteractiveScheduleUad.Api.Models;
using InteractiveScheduleUad.Api.Models.Dtos;
using InteractiveScheduleUad.Api.Repositories.Contracts;
using InteractiveScheduleUad.Api.Services.Contracts;

namespace InteractiveScheduleUad.Api.Services;

public class DepartmentService : IDepartmentService
{
    private readonly IDepartmentRepository _departmentRepository;

    public DepartmentService(IDepartmentRepository departmentRepository)
    {
        _departmentRepository = departmentRepository;
    }

    public async Task<Result<Department>> CreateAsync(TeacherDepartmentForWriteDto departmentCreateDto)
    {
        Department department = DepartmentMapper.DepartmentForWriteDtoToDepartment(departmentCreateDto);

        await _departmentRepository.InsertAsync(department);
        await _departmentRepository.SaveChangesAsync();

        return department;
    }

    public async Task<Result> DeleteAsync(int id)
    {
        var department = await _departmentRepository.GetByIdAsync(id);

        if (department is not null)
        {
            _departmentRepository.Delete(department);
            await _departmentRepository.SaveChangesAsync();

            return Result.Ok();
        }
        else
            return new NotFoundError(nameof(Department));
    }

    public async Task<Result<IEnumerable<Department>>> GetAllAsync()
    {
        var departments = await _departmentRepository.GetAllAsync();

        return Result.Ok(departments);
    }

    public async Task<Result<Department>> GetByIdAsync(int id)
    {
        var department = await _departmentRepository.GetByIdAsync(id);

        if (department is not null)
            return department;
        else
            return new NotFoundError(nameof(Department));
    }

    public async Task<Result> UpdateAsync(int id, TeacherDepartmentForWriteDto departmentCreateDto)
    {
        var department = await _departmentRepository.GetByIdAsync(id);

        if (department is not null)
        {
            DepartmentMapper.DepartmentForWriteDtoToDepartment(departmentCreateDto, department);

            _departmentRepository.Update(department);
            await _departmentRepository.SaveChangesAsync();

            return Result.Ok();
        }
        else
            return new NotFoundError(nameof(Department));
    }
}