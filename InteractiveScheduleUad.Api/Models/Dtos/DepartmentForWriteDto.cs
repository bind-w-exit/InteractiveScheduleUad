namespace InteractiveScheduleUad.Api.Models.Dtos;

public class DepartmentForWriteDto
{
    public required string Name { get; set; }

    public required string Abbreviation { get; set; }

    public string? Link { get; set; }
}