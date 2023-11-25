using InteractiveScheduleUad.Api.Models;
using InteractiveScheduleUad.Api.Models.Dtos;
using Riok.Mapperly.Abstractions;

namespace InteractiveScheduleUad.Api.Mappers;

[Mapper]
public static partial class StudentsGroupMapper
{
    [MapperIgnoreSource(nameof(StudentsGroup.FullContexts))]
    public static partial StudentsGroupForReadDto StudentsGroupToStudentsGroupForReadDto(StudentsGroup studentsGroup);

    [MapperIgnoreSource(nameof(StudentsGroup.FullContexts))]
    public static partial void StudentsGroupToStudentsGroupForReadDto(StudentsGroup studentsGroup, StudentsGroupForReadDto studentsGroupForReadDto);

    [MapperIgnoreSource(nameof(StudentsGroup.Id))]
    public static partial StudentsGroupWithSchedulesDto StudentsGroupToStudentsGroupForWriteDto(StudentsGroup studentsGroup);
}