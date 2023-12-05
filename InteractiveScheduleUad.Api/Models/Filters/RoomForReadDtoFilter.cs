using AutoFilterer.Attributes;
using AutoFilterer.Enums;
using AutoFilterer.Types;

namespace InteractiveScheduleUad.Api.Models.Filters;

public class RoomForReadDtoFilter : FilterBase
{
    public int? Id { get; set; }

    [StringFilterOptions(StringFilterOption.Contains)]
    public string? Name { get; set; }
}