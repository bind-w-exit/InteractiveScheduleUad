using AutoFilterer.Attributes;
using AutoFilterer.Enums;
using AutoFilterer.Types;

namespace InteractiveScheduleUad.Api.Models.Filters;

public class StudentsGroupForReadDtoFilter : FilterBase
{
    public int? Id { get; set; }

    [StringFilterOptions(StringFilterOption.Equals)]
    public string? Name { get; set; }
}