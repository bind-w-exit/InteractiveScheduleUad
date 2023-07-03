using System.ComponentModel.DataAnnotations;

namespace InteractiveScheduleUad.Api.Models.Dtos;

public class DepartmentForWriteDto
{
    [Required]
    public string Name { get; set; }

    [Required]
    public string Abbreviation { get; set; }

    public string Link { get; set; }
}