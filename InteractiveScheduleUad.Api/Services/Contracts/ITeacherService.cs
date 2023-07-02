using InteractiveScheduleUad.Api.Models;
using InteractiveScheduleUad.Api.Models.Dtos;

namespace InteractiveScheduleUad.Api.Services.Contracts;

public interface ITeacherService
{
    Task<Teacher> CreateAsync(TeacherForWriteDto teacherForWriteDto);

    Task<IEnumerable<Teacher>> GetAllAsync();

    Task<Teacher?> GetByIdAsync(int id);

    Task<bool> UpdateAsync(int id, TeacherForWriteDto teacherForWriteDto);

    Task<bool> DeleteAsync(int id);
}