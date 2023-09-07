using FluentResults;
using InteractiveScheduleUad.Api.Models;
using InteractiveScheduleUad.Api.Models.Dtos;

namespace InteractiveScheduleUad.Api.Services.Contracts;

public interface ITeacherService
{
    Task<Result<Teacher>> CreateAsync(TeacherForWriteDto teacherForWriteDto);

    Task<Result<IEnumerable<Teacher>>> GetAllAsync();

    Task<Result<Teacher>> GetByIdAsync(int id);

    Task<Result> UpdateAsync(int id, TeacherForWriteDto teacherForWriteDto);

    Task<Result> DeleteAsync(int id);
}