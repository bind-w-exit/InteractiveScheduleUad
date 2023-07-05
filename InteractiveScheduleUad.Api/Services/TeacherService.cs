using InteractiveScheduleUad.Api.Models;
using InteractiveScheduleUad.Api.Models.Dtos;
using InteractiveScheduleUad.Api.Repositories.Contracts;
using InteractiveScheduleUad.Api.Services.Contracts;

namespace InteractiveScheduleUad.Api.Services;

public class TeacherService : ITeacherService
{
    private readonly ITeacherRepository _teacherRepository;
    private readonly IDepartmentRepository _departmentRepository;

    public TeacherService(ITeacherRepository teacherRepository, IDepartmentRepository departmentRepository)
    {
        _teacherRepository = teacherRepository;
        _departmentRepository = departmentRepository;
    }

    public async Task<Teacher> CreateAsync(TeacherForWriteDto teacherForWriteDto)
    {
        Teacher teacher = await TeacherForWriteDtoToTeacher(teacherForWriteDto);

        await _teacherRepository.InsertAsync(teacher);
        await _teacherRepository.SaveChangesAsync();

        return teacher;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var teacher = await _teacherRepository.GetByIdAsync(id);
        if (teacher is not null)
        {
            _teacherRepository.Delete(teacher);
            await _teacherRepository.SaveChangesAsync();
            return true;
        }
        return false;
    }

    public async Task<IEnumerable<Teacher>> GetAllAsync()
    {
        return await _teacherRepository.GetAllAsync(true);
    }

    public async Task<Teacher?> GetByIdAsync(int id)
    {
        return await _teacherRepository.GetByIdAsync(id);
    }

    public async Task<bool> UpdateAsync(int id, TeacherForWriteDto teacherForWriteDto)
    {
        var teacherFromDb = await _teacherRepository.GetByIdAsync(id);
        if (teacherFromDb is not null)
        {
            await TeacherForWriteDtoToTeacher(teacherForWriteDto, teacherFromDb);

            _teacherRepository.Update(teacherFromDb);
            await _teacherRepository.SaveChangesAsync();

            return true;
        }
        return false;
    }

    private async Task TeacherForWriteDtoToTeacher(TeacherForWriteDto teacherForWriteDto, Teacher teacher)
    {
        var department = await _departmentRepository.GetByIdAsync(teacherForWriteDto.DepartmentId);

        teacher.Department = department;
        teacher.Email = teacherForWriteDto.Email;
        teacher.FirstName = teacherForWriteDto.FirstName;
        teacher.LastName = teacherForWriteDto.LastName;
        teacher.MiddleName = teacherForWriteDto.MiddleName;
        teacher.Qualifications = teacherForWriteDto.Qualifications;
    }

    private async Task<Teacher> TeacherForWriteDtoToTeacher(TeacherForWriteDto teacherForWriteDto)
    {
        var department = await _departmentRepository.GetByIdAsync(teacherForWriteDto.DepartmentId);

        return new()
        {
            Department = department,
            Email = teacherForWriteDto.Email,
            FirstName = teacherForWriteDto.FirstName,
            LastName = teacherForWriteDto.LastName,
            MiddleName = teacherForWriteDto.MiddleName,
            Qualifications = teacherForWriteDto.Qualifications,
        };
    }
}