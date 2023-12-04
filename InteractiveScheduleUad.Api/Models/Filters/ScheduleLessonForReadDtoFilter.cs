using AutoFilterer.Types;
using System.ComponentModel.DataAnnotations;

namespace InteractiveScheduleUad.Api.Models.Filters;

public class ScheduleLessonForReadDtoFilter : FilterBase
{
    public int? Id { get; set; }

    // foreign keys

    public int? LessonId { get; set; }
    public int? FullContextId { get; set; }

    // navigations
    public virtual LessonForReadDtoFilter? Lesson { get; set; }

    public virtual FullContextForReadDtoFilter FullContext { get; set; }
}