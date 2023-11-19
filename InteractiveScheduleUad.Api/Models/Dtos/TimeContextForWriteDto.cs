namespace InteractiveScheduleUad.Api.Models.Dtos;

public class TimeContextForWriteDto
{
    public int LessonIndex { get; set; }
    public int WeekDay { get; set; }
    public int WeekIndex { get; set; }
}