using Microsoft.EntityFrameworkCore;

namespace InteractiveScheduleUad.Api.Models;

[Index(nameof(Name), IsUnique = true)]
public class Room : Entity
{
    public string Name { get; set; }

    // foreign keys

    public int? LessonId { get; set; }

    // navigations

    public virtual IEnumerable<Lesson>? Lesson { get; set; }

    public override string ToString()
    {
        return Name;
    }
}