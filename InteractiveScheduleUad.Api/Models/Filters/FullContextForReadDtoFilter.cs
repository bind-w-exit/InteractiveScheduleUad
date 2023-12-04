using AutoFilterer.Types;
using InteractiveScheduleUad.Api.Models.Dtos;

namespace InteractiveScheduleUad.Api.Models.Filters;

public class FullContextForReadDtoFilter : FilterBase
{
    public StudentsGroupForReadDtoFilter? StudentsGroup { get; set; }
    public TimeContextForReadDtoFilter? TimeContext { get; set; }
}