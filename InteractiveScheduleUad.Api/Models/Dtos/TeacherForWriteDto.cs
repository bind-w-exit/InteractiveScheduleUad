namespace InteractiveScheduleUad.Api.Models.Dtos;

public class TeacherForWriteDto
{
    public required string FirstName { get; set; }

    public required string LastName { get; set; }

    public string? MiddleName { get; set; }

    public string? Email { get; set; }

    public string? Qualifications { get; set; }

    public int? DepartmentId { get; set; }
}