namespace InteractiveScheduleUad.Api.Models.Dtos;

public class TeacherForReadDto
{
    public int Id { get; set; }

    // primitives

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string? MiddleName { get; set; }

    public string? Email { get; set; }

    public string? Qualifications { get; set; }

    // navigations

    public virtual TeacherDepartmentForReadDto? Department { get; set; }

    // ignore
}