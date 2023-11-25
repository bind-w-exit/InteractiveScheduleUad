using InteractiveScheduleUad.Api.Models.Dtos;

namespace InteractiveScheduleUad.Api.Models;

public class FullContextForWriteDto
{
    public int StudentsGroupId { get; set; }
    public TimeContextForWriteDto TimeContext { get; set; }
}