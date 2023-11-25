using InteractiveScheduleUad.Api.Models;
using InteractiveScheduleUad.Api.Models.Dtos;
using Riok.Mapperly.Abstractions;

namespace InteractiveScheduleUad.Api.Mappers;

[Mapper]
public static partial class LessonMapper
{
    //[MapperIgnoreSource(nameof(Lesson.Id))]
    public static partial LessonForReadDto LessonToLessonForReadDto(Lesson lesson);

    [MapperIgnoreTarget(nameof(Lesson.Id))]
    public static partial Lesson LessonForWriteDtoToLesson(LessonForWriteDto lesson);

    //[MapperIgnoreTarget(nameof(Lesson.Id))]
    //public static partial Lesson LessonWithRelatedEntitiesForWriteDtoToLesson(LessonWithRelatedEntitiesForWriteDto lesson);

    private static string ClassTypeToString(ClassType classType)
    {
        return classType switch
        {
            ClassType.Lecture => "лек.",
            ClassType.Practical => "практ.",
            ClassType.Laboratory => "лаб.",

            _ => classType.ToString(),
        };
    }

    private static string RoomToString(Room room)
    {
        return room.Name;
    }

    private static Subject StringToSubject(string subject)
    {
        return new Subject { Name = subject };
    }
}