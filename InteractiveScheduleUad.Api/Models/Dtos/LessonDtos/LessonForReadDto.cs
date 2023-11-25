namespace InteractiveScheduleUad.Api.Models.Dtos;

public class LessonForReadDto
{
    public int Id { get; set; }

    public Subject? Subject { get; set; }

    public TeacherForReadDto? Teacher { get; set; }

    public RoomForReadDto? Room { get; set; }

    public string? ClassType { get; set; }
}