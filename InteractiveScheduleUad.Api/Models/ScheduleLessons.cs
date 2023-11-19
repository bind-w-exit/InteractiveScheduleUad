namespace InteractiveScheduleUad.Api.Models;

// different from just lessons
public class ScheduleLesson
{
    public int Id { get; set; }

    public int StudentsGroupId { get; set; }
    public int LessonId { get; set; }
    public int TimeContextId { get; set; }

    // navigations

    public IEnumerable<StudentsGroup>? StudentsGroups { get; set; }
    public IEnumerable<Lesson>? Lessons { get; set; }
    public IEnumerable<TimeContext>? TimeContexts { get; set; }
}