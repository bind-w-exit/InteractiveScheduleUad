using InteractiveScheduleUad.Api.Models;
using InteractiveScheduleUad.Api.Models.Dtos;
using Riok.Mapperly.Abstractions;

namespace InteractiveScheduleUad.Api.Mappers;

[Mapper]
public static partial class StudentsGroupMapper
{
    [MapperIgnoreSource(nameof(StudentsGroup.FirstWeekSchedules))]
    [MapperIgnoreSource(nameof(StudentsGroup.SecondWeekSchedules))]
    public static partial StudentsGroupForReadDto StudentsGroupToStudentsGroupForListDto(StudentsGroup studentsGroup);

    [MapperIgnoreSource(nameof(StudentsGroup.Id))]
    public static partial StudentsGroupForWriteDto StudentsGroupToStudentsGroupDto(StudentsGroup studentsGroup);

    private static WeekScheduleForReadDto WeekScheduleToWeekScheduleDto(WeekSchedule weekSchedule)
        => WeekScheduleMapper.WeekScheduleToWeekScheduleDto(weekSchedule);
}