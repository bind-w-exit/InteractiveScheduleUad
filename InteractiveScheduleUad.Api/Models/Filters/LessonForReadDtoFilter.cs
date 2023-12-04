using AutoFilterer.Types;
using InteractiveScheduleUad.Api.Models.Dtos;

namespace InteractiveScheduleUad.Api.Models.Filters;

public class LessonForReadDtoFilter : FilterBase
{
    public int? Id { get; set; }

    public SubjectForReadDtoFilter? Subject { get; set; }

    public TeacherForReadDtoFilter? Teacher { get; set; }

    public RoomForReadDtoFilter? Room { get; set; }

    public string? ClassType { get; set; }
}