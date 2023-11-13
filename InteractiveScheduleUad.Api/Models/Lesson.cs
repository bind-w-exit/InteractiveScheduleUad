namespace InteractiveScheduleUad.Api.Models;

public class Lesson
{
    public int Id { get; set; }

    // lesson number/index within list of lessons for a day
    public int Sequence { get; set; }

    public Subject? Subject { get; set; }

    public Teacher? Teacher { get; set; }

    public Room? Room { get; set; }

    public ClassType? ClassType { get; set; } = Models.ClassType.Lecture;
}