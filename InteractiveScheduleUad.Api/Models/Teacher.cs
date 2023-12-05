using Microsoft.EntityFrameworkCore;

namespace InteractiveScheduleUad.Api.Models;

[Index(nameof(FirstName), nameof(LastName), IsUnique = true)]
public class Teacher : Entity
{
    public int Id { get; set; }

    // primitives

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string? MiddleName { get; set; }

    public string? Email { get; set; }

    public string? Qualifications { get; set; }

    // foreign keys

    public int? DepartmentId { get; set; }
    public int? LessonId { get; set; }

    // navigations

    public virtual Department? Department { get; set; }
    public virtual IEnumerable<Lesson>? Lesson { get; set; }

    public override string ToString()
    {
        return $"{FirstName} {LastName} {MiddleName}";
    }
}