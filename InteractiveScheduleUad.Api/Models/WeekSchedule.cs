namespace InteractiveScheduleUad.Api.Models;

public class WeekSchedule
{
    public int Id { get; set; }
    public IEnumerable<Lesson>? Sunday { get; set; }
    public IEnumerable<Lesson>? Monday { get; set; }
    public IEnumerable<Lesson>? Tuesday { get; set; }
    public IEnumerable<Lesson>? Wednesday { get; set; }
    public IEnumerable<Lesson>? Thursday { get; set; }
    public IEnumerable<Lesson>? Friday { get; set; }
    public IEnumerable<Lesson>? Saturday { get; set; }
}