using InteractiveScheduleUad.Api.Models;

namespace InteractiveScheduleUad.E2ETests.Constants;

public static class ApiEndpoints
{
    public const string scheduleLessonsEndpoint = "ScheduleLesson";
    public const string groupsEndpoint = nameof(StudentsGroup);
    public const string lessonsEndpoint = nameof(Lesson);
    public const string subjectsEndpoint = nameof(Subject);
    public const string roomsEndpoint = nameof(Room);

    public const string teachersEndpoint = nameof(Teacher);
    public const string teacherDepartmentEndpoint = $"Teacher{nameof(Department)}";
}