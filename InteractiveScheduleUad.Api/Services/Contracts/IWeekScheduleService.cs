using FluentResults;
using InteractiveScheduleUad.Api.Models.Dtos;

namespace InteractiveScheduleUad.Api.Services.Contracts;

public interface IWeekScheduleService
{
    Task<Result<WeekScheduleForReadDto>> CreateAsync(int studentsGroupId, WeekScheduleForWriteDto weekScheduleForWriteDto, bool IsSecondWeek);

    Task<Result<StudentsGroupForWriteDto>> GetByIdAsync(int studentsGroupId);

    Task<Result> DeleteAsync(int studentsGroupId, bool IsSecondWeek);
}