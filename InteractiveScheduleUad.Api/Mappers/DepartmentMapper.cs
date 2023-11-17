using InteractiveScheduleUad.Api.Models;
using InteractiveScheduleUad.Api.Models.Dtos;
using Riok.Mapperly.Abstractions;

namespace InteractiveScheduleUad.Api.Mappers;

[Mapper]
public static partial class DepartmentMapper
{
    [MapperIgnoreTarget(nameof(Department.Id))]
    public static partial Department DepartmentForWriteDtoToDepartment(TeacherDepartmentForWriteDto departmentForWriteDto);

    [MapperIgnoreTarget(nameof(Department.Id))]
    public static partial void DepartmentForWriteDtoToDepartment(TeacherDepartmentForWriteDto departmentForWriteDto, Department department);
}