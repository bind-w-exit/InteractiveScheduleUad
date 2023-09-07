using InteractiveScheduleUad.Api.Models;
using InteractiveScheduleUad.Api.Models.Dtos;
using Riok.Mapperly.Abstractions;

namespace InteractiveScheduleUad.Api.Mappers;

[Mapper]
public static partial class TeacherMapper
{
    [MapperIgnoreSource(nameof(TeacherForWriteDto.DepartmentId))]
    [MapperIgnoreTarget(nameof(Teacher.Id))]
    [MapperIgnoreTarget(nameof(Teacher.Department))]
    public static partial Teacher TeacherForWriteDtoToTeacher(TeacherForWriteDto teacherForWriteDto);

    [MapperIgnoreSource(nameof(TeacherForWriteDto.DepartmentId))]
    [MapperIgnoreTarget(nameof(Teacher.Id))]
    [MapperIgnoreTarget(nameof(Teacher.Department))]
    public static partial void TeacherForWriteDtoToTeacher(TeacherForWriteDto teacherForWriteDto, Teacher teacher);
}