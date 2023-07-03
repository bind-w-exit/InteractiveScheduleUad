using InteractiveScheduleUad.Api.Models;
using InteractiveScheduleUad.Api.Models.Dtos;
using Riok.Mapperly.Abstractions;

namespace InteractiveScheduleUad.Api.Mappers;

[Mapper]
public static partial class WeekScheduleMapper
{
    [MapperIgnoreSource(nameof(WeekSchedule.Id))]
    public static partial WeekScheduleForReadDto WeekScheduleToWeekScheduleForReadDto(WeekSchedule weekSchedule);

    private static LessonForReadDto LessonToLessonForReadDto(Lesson lesson) => LessonMapper.LessonToLessonForReadDto(lesson);
}