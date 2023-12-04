using InteractiveScheduleUad.Api.Models.Filters;

namespace InteractiveScheduleUad.Api.Models.Dtos;

// different from just lessons
public class ScheduleLessonForReadDto
{
    public int Id { get; set; }
    public LessonForReadDto? Lesson { get; set; }

    public FullContextForReadDto? FullContext { get; set; }
}