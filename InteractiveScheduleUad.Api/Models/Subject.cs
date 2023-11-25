using Microsoft.EntityFrameworkCore;

namespace InteractiveScheduleUad.Api.Models;

[Index(nameof(Name), IsUnique = true)]
public class Subject
{
    public int Id { get; set; }

    public string Name { get; set; }

    public override string ToString()
    {
        return Name;
    }
}