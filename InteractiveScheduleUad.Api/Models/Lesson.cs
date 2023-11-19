namespace InteractiveScheduleUad.Api.Models;

public class Lesson
{
    public int Id { get; set; }

    public ClassType? ClassType { get; set; } = Models.ClassType.Lecture;

    // navigations

    public Subject? Subject { get; set; }

    public Teacher? Teacher { get; set; }

    public Room? Room { get; set; }

    public IEnumerable<ScheduleLesson>? ScheduleLessons { get; set; }
}