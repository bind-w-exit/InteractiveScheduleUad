using InteractiveScheduleUad.Api.Models;
using InteractiveScheduleUad.Api.Models.Dtos;
using Riok.Mapperly.Abstractions;

namespace InteractiveScheduleUad.Api.Mappers;

[Mapper]
public static partial class TeacherMapper
{
    [MapperIgnoreTarget(nameof(Teacher.Id))]
    [MapperIgnoreTarget(nameof(Teacher.Department))]
    public static partial Teacher TeacherForWriteDtoToTeacher(TeacherForWriteDto teacherForWriteDto);

    [MapperIgnoreTarget(nameof(Teacher.Id))]
    [MapperIgnoreTarget(nameof(Teacher.Department))]
    public static partial void TeacherForWriteDtoToTeacher(TeacherForWriteDto teacherForWriteDto, Teacher teacher);

    [MapperIgnoreSource(nameof(Teacher.DepartmentId))]
    [MapperIgnoreSource(nameof(Teacher.LessonId))] // this stuff bloats the response object too much
    [MapperIgnoreSource(nameof(Teacher.Lesson))]
    public static partial TeacherForReadDto TeacherToTeacherForReadDto(Teacher teacher);
}