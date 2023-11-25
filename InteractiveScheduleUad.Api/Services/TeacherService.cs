using FluentResults;
using InteractiveScheduleUad.Api.Errors;
using InteractiveScheduleUad.Api.Mappers;
using InteractiveScheduleUad.Api.Models;
using InteractiveScheduleUad.Api.Models.Dtos;
using InteractiveScheduleUad.Api.Repositories.Contracts;
using InteractiveScheduleUad.Api.Services.Contracts;
using System.Security.Cryptography;
using static InteractiveScheduleUad.Api.Utilities.Utls;

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

    public async Task<Result<Teacher>> CreateAsync(TeacherForWriteDto teacherForWriteDto)
    {
        Teacher teacher = TeacherMapper.TeacherForWriteDtoToTeacher(teacherForWriteDto);

        await _teacherRepository.InsertAsync(teacher);
        await _teacherRepository.SaveChangesAsync();

        return teacher;
    }

    public async Task<Result> DeleteAsync(int id)
    {
        var teacher = await _teacherRepository.GetByIdAsync(id);

        if (teacher is not null)
        {
            _teacherRepository.Delete(teacher);
            await _teacherRepository.SaveChangesAsync();

            return Result.Ok();
        }
        else
            return new NotFoundError(nameof(Teacher));
    }

    public async Task<Result<IEnumerable<Teacher>>> GetAllAsync()
    {
        var teachers = await _teacherRepository.GetAllAsync(true);

        return Result.Ok(teachers);
    }

    public async Task<Result<Teacher>> GetByIdAsync(int id)
    {
        var teacher = await _teacherRepository.GetByIdAsync(id);

        if (teacher is not null)
            return teacher;
        else
            return new NotFoundError(nameof(Teacher));
    }

    public async Task<Result> UpdateAsync(int id, TeacherForWriteDto teacherForWriteDto)
    {
        var teacher = await _teacherRepository.GetByIdAsync(id);

        if (teacher is not null)
        {
            TeacherMapper.TeacherForWriteDtoToTeacher(teacherForWriteDto, teacher);
            teacher.Department = await _departmentRepository.GetByIdAsync(teacherForWriteDto.DepartmentId);

            _teacherRepository.Update(teacher);
            await _teacherRepository.SaveChangesAsync();

            return Result.Ok();
        }
        else
            return new NotFoundError(nameof(Teacher));
    }
}