using AutoFilterer.Attributes;
using AutoFilterer.Enums;
using AutoFilterer.Types;
using InteractiveScheduleUad.Api.Models.Dtos;

namespace InteractiveScheduleUad.Api.Models.Filters;

public class TeacherForReadDtoFilter : FilterBase
{
    public int? Id { get; set; }

    // primitives

    [StringFilterOptions(StringFilterOption.Contains)]
    public string? FirstName { get; set; } = null!;

    [StringFilterOptions(StringFilterOption.Contains)]
    public string? LastName { get; set; } = null!;

    [StringFilterOptions(StringFilterOption.Contains)]
    public string? MiddleName { get; set; }

    [StringFilterOptions(StringFilterOption.Contains)]
    public string? Email { get; set; }

    [StringFilterOptions(StringFilterOption.Contains)]
    public string? Qualifications { get; set; }

    // navigations
    // TODO: add department
}