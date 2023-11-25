namespace InteractiveScheduleUad.Api.Models;

public class TimeContextForReadDto
{
    public int LessonIndex { get; set; }
    public DayOfWeek WeekDay { get; set; }
    public int WeekIndex { get; set; }
}