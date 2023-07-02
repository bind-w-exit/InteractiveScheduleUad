namespace InteractiveScheduleUad.Api.Models.Dtos;

public class WeekScheduleForWriteDto
{
    public IEnumerable<LessonForWriteDto>? Sunday { get; set; }
    public IEnumerable<LessonForWriteDto>? Monday { get; set; }
    public IEnumerable<LessonForWriteDto>? Tuesday { get; set; }
    public IEnumerable<LessonForWriteDto>? Wednesday { get; set; }
    public IEnumerable<LessonForWriteDto>? Thursday { get; set; }
    public IEnumerable<LessonForWriteDto>? Friday { get; set; }
    public IEnumerable<LessonForWriteDto>? Saturday { get; set; }
}