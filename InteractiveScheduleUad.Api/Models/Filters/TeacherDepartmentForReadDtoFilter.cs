using AutoFilterer.Types;

namespace InteractiveScheduleUad.Api.Models.Filters;

public class TeacherDepartmentForReadDtoFilter : FilterBase
{
    public int? Id { get; set; }

    public string? Name { get; set; }
    public string? Abbreviation { get; set; }
    public string? Link { get; set; }
}