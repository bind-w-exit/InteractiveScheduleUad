using AutoFilterer.Types;

namespace InteractiveScheduleUad.Api.Models.Filters;

public class RoomForReadDtoFilter : FilterBase
{
    public int? Id { get; set; }
    public string? Name { get; set; }
}