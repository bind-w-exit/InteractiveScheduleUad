namespace InteractiveScheduleUad.Api.Models;

public class Teacher
{
    public int Id { get; set; }

    public required string FirstName { get; set; }

    public required string LastName { get; set; }

    public string? MiddleName { get; set; }

    public string? Email { get; set; }

    public string? Qualifications { get; set; }

    public Department? Department { get; set; }

    public override string ToString()
    {
        return $"{FirstName} {LastName} {MiddleName}";
    }
}