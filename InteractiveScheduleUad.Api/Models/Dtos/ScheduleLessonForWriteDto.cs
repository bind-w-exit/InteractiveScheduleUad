namespace InteractiveScheduleUad.Api.Models.Dtos;

// everything comes in as Id. Except TimeContext
public class ScheduleLessonForWriteDto
{
    public int LessonId { get; set; }

    public FullContextForWriteDto FullContext { get; set; }
}