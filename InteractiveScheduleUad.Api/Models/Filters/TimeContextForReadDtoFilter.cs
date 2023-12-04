using AutoFilterer.Types;

namespace InteractiveScheduleUad.Api.Models.Filters;

public class TimeContextForReadDtoFilter : FilterBase
{
    public int? LessonIndex { get; set; }
    public DayOfWeek? WeekDay { get; set; }
    public int? WeekIndex { get; set; }
}