using InteractiveScheduleUad.Api.Models.Dtos;

namespace InteractiveScheduleUad.Api.Models;

public class FullContextForReadDto
{
    public StudentsGroupForReadDto StudentsGroup { get; set; }
    public TimeContextForReadDto TimeContext { get; set; }
}