namespace InteractiveScheduleUad.Api.Models;

public class Department
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public required string Abbreviation { get; set; }

    public string? Link { get; set; }
}