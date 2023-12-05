namespace InteractiveScheduleUad.Api.Models;

public class Department : Entity
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Abbreviation { get; set; }

    public string? Link { get; set; }
}