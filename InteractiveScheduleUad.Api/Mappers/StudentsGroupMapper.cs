using InteractiveScheduleUad.Api.Models;
using InteractiveScheduleUad.Api.Models.Dtos;
using Riok.Mapperly.Abstractions;

namespace InteractiveScheduleUad.Api.Mappers;

[Mapper]
public static partial class StudentsGroupMapper
{
    [MapperIgnoreSource(nameof(StudentsGroup.FirstWeekSchedule))]
    [MapperIgnoreSource(nameof(StudentsGroup.SecondWeekSchedule))]
    public static partial StudentsGroupForReadDto StudentsGroupToStudentsGroupForReadDto(StudentsGroup studentsGroup);

    [MapperIgnoreSource(nameof(StudentsGroup.FirstWeekSchedule))]
    [MapperIgnoreSource(nameof(StudentsGroup.SecondWeekSchedule))]
    public static partial void StudentsGroupToStudentsGroupForReadDto(StudentsGroup studentsGroup, StudentsGroupForReadDto studentsGroupForReadDto);

    [MapperIgnoreSource(nameof(StudentsGroup.Id))]
    public static partial StudentsGroupWithSchedulesDto StudentsGroupToStudentsGroupForWriteDto(StudentsGroup studentsGroup);

    private static WeekScheduleForReadDto WeekScheduleToWeekScheduleForReadDto(WeekSchedule weekSchedule)
        => WeekScheduleMapper.WeekScheduleToWeekScheduleForReadDto(weekSchedule);
}