namespace InteractiveScheduleUad.Api.Models.Dtos;

public class LessonForWriteDto
{
    public int Sequence { get; set; }

    public int SubjectId { get; set; }

    public int TeacherId { get; set; }

    public int RoomId { get; set; }

    public ClassType ClassType { get; set; }
}