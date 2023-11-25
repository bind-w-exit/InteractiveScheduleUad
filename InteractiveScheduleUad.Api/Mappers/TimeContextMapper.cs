using InteractiveScheduleUad.Api.Models;
using InteractiveScheduleUad.Api.Models.Dtos;
using Riok.Mapperly.Abstractions;

namespace InteractiveScheduleUad.Api.Mappers;

[Mapper]
public static partial class TimeContextMapper
{
    [MapperIgnoreTarget(nameof(TimeContext.Id))]
    public static partial TimeContext TimeContextForWriteDtoToTimeContext(TimeContextForWriteDto authorForWriteDto);
}