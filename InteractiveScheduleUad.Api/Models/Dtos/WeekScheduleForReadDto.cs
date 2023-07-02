namespace InteractiveScheduleUad.Api.Models.Dtos;

public class WeekScheduleForReadDto
{
    public IEnumerable<LessonForReadDto>? Sunday { get; set; }
    public IEnumerable<LessonForReadDto>? Monday { get; set; }
    public IEnumerable<LessonForReadDto>? Tuesday { get; set; }
    public IEnumerable<LessonForReadDto>? Wednesday { get; set; }
    public IEnumerable<LessonForReadDto>? Thursday { get; set; }
    public IEnumerable<LessonForReadDto>? Friday { get; set; }
    public IEnumerable<LessonForReadDto>? Saturday { get; set; }
}