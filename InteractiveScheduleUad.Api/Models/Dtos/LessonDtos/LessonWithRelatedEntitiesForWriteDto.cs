namespace InteractiveScheduleUad.Api.Models.Dtos;

public class LessonWithRelatedEntitiesForWriteDto
{
    public ClassType? ClassType { get; set; } = Models.ClassType.Lecture;


    public string? Subject { get; set; }

    public TeacherForWriteDto? Teacher { get; set; }

    public RoomForWriteDto? Room { get; set; }

}
