namespace InteractiveScheduleUad.Api.Models;

public class TimeContext
{
    public int Id { get; set; }

    public int LessonIndex { get; set; }
    public int WeekDay { get; set; }
    public int WeekIndex { get; set; }

    public IEnumerable<ScheduleLesson>? ScheduleLessons { get; set; }
}