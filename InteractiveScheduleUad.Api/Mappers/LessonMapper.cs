using InteractiveScheduleUad.Api.Models;
using InteractiveScheduleUad.Api.Models.Dtos;
using Riok.Mapperly.Abstractions;

namespace InteractiveScheduleUad.Api.Mappers;

[Mapper]
public static partial class LessonMapper
{
    [MapperIgnoreSource(nameof(Lesson.Id))]
    public static partial LessonForReadDto LessonToLessonForReadDto(Lesson lesson);

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
}