namespace InteractiveScheduleUad.Api.Models.Dtos;

public class LessonForReadDto
{
    public int Sequence { get; set; }

    public required string Subject { get; set; }

    public string? Teacher { get; set; }

    public string? Room { get; set; }

    public string? ClassType { get; set; }
}