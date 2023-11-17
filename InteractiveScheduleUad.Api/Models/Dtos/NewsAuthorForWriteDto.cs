using System.ComponentModel.DataAnnotations;

namespace InteractiveScheduleUad.Api.Models.Dtos;

public class NewsAuthorForWriteDto
{
    [Required]
    [StringLength(200)]
    public string FirstName { get; set; }

    [Required]
    [StringLength(200)]
    public string LastName { get; set; }

    [Required]
    [StringLength(150)]
    public string NickName { get; set; }

    [Required]
    [StringLength(256)]
    public string Email { get; set; }

    [Required]
    [StringLength(1000)]
    public string? Bio { get; set; }
}