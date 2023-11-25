using InteractiveScheduleUad.Api.Models;
using InteractiveScheduleUad.Api.Models.Dtos;
using Riok.Mapperly.Abstractions;

namespace InteractiveScheduleUad.Api.Mappers;

[Mapper]
public static partial class FullContextMapper
{
    //[MapperIgnoreTarget(nameof(TimeContext.Id))]
    public static partial FullContextJunction FullContextForWriteDtoToFullContext(FullContextForWriteDto fullContextForWrite);

    public static partial FullContextForReadDto FullContextToFullContextForReadDto(FullContextJunction fullContext);

    private static StudentsGroup StudentsGroupStringToStudentsGroup(string studentsGroupForWrite)
    {
        return new StudentsGroup { GroupName = studentsGroupForWrite };
    }
}