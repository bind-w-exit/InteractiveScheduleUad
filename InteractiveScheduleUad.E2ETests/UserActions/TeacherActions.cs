using InteractiveScheduleUad.Api.Models.Dtos;
using InteractiveScheduleUad.Api.Models;
using InteractiveScheduleUad.E2ETests.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using InteractiveScheduleUad.E2ETests.Constants;
using InteractiveScheduleUad.Core.Extensions;

namespace InteractiveScheduleUad.E2ETests.UserActions;

public static class TeacherActions
{
    public static Teacher CreateTeacher(RawTeacher rawTeacher, RestClient client)
    {
        // Full name = Прізвище Ім'я По батькові
        var nameBits = rawTeacher.FullName.Split(' ');
        var lastName = nameBits.First();
        var firstName = nameBits[1];
        var middleName = nameBits.Length == 3 ? nameBits[2] : null; // aka patronymic

        var email = rawTeacher.Email;

        // ensure that department exists

        TeacherDepartmentForWriteDto newDepartment = new()
        {
            Name = rawTeacher.DepartmentFullName,
            Abbreviation = rawTeacher.DepartmentAbbreviation,
            Link = rawTeacher.DepartmentLink
        };
        var department = client.EnsureExists<Department, TeacherDepartmentForWriteDto>
            (ApiEndpoints.teacherDepartmentEndpoint, null, newDepartment, (d) => d.Name == newDepartment.Name);
        var departmentId = department.Id;

        // construct teacher object

        var teacher = new TeacherForWriteDto()
        {
            FirstName = firstName,
            LastName = lastName,
            MiddleName = middleName,
            Email = email,
            Qualifications = rawTeacher.Qualification,
            DepartmentId = departmentId
        };

        return client.PostJson<TeacherForWriteDto, Teacher>(ApiEndpoints.teachersEndpoint, teacher);
    }
}