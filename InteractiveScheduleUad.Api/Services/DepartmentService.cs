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

    public async Task<Department> CreateAsync(DepartmentForWriteDto departmentCreateDto)
    {
        Department department = DepartmentMapper.DepartmentCreateDtoToDepartment(departmentCreateDto);

        await _departmentRepository.InsertAsync(department);
        await _departmentRepository.SaveChangesAsync();

        return department;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var department = await _departmentRepository.GetByIdAsync(id);
        if (department is not null)
        {
            _departmentRepository.Delete(department);
            await _departmentRepository.SaveChangesAsync();
            return true;
        }
        return false;
    }

    public async Task<IEnumerable<Department>> GetAllAsync()
    {
        return await _departmentRepository.GetAllAsync();
    }

    public async Task<Department?> GetByIdAsync(int id)
    {
        return await _departmentRepository.GetByIdAsync(id);
    }

    public async Task<bool> UpdateAsync(int id, DepartmentForWriteDto departmentCreateDto)
    {
        var departmentFromDb = await _departmentRepository.GetByIdAsync(id);
        if (departmentFromDb is not null)
        {
            DepartmentMapper.DepartmentCreateDtoToDepartment(departmentCreateDto, departmentFromDb);

            _departmentRepository.Update(departmentFromDb);
            await _departmentRepository.SaveChangesAsync();

            return true;
        }
        return false;
    }
}