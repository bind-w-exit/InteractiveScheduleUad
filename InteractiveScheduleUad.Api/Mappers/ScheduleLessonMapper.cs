using InteractiveScheduleUad.Api.Models;
using InteractiveScheduleUad.Api.Models.Dtos;
using Riok.Mapperly.Abstractions;

namespace InteractiveScheduleUad.Api.Mappers;

[Mapper]
public static partial class ScheduleLessonMapper
{
    [MapperIgnoreTarget(nameof(ScheduleLessonJunction.Id))]
    public static partial ScheduleLessonJunction ScheduleLessonForWriteDtoToScheduleLessonJunction(ScheduleLessonForWriteDto authorForWriteDto);

    public static partial ScheduleLessonForReadDto ScheduleLessonToScheduleLessonForRead(ScheduleLessonJunction scheduleLesson);

    private static Subject StringToSubject(string subject)
    {
        return new Subject { Name = subject };
    }

    private static StudentsGroup StringToStudentsGroup(string group)
    {
        return new StudentsGroup { Name = group };
    }

    //private static RoomForRead StringToStudentsGroup(string group)
    //{
    //    return new StudentsGroup { GroupName = group };
    //}
}