using InteractiveScheduleUad.Api.Models.Dtos;

namespace InteractiveScheduleUad.Api.Services.Contracts;

public interface IWeekScheduleService
{
    Task<WeekScheduleForReadDto?> CreateAsync(int studentsGroupId, WeekScheduleForWriteDto weekScheduleForWriteDto, bool IsSecondWeek);

    Task<StudentsGroupForWriteDto?> GetByIdAsync(int studentsGroupId);

    Task<bool> DeleteAsync(int studentsGroupId, bool IsSecondWeek);
}