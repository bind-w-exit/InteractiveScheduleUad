using System.ComponentModel.DataAnnotations;

namespace InteractiveScheduleUad.Api.Models.Dtos;

public class ArticleForWriteDto
{
    [Required]
    [StringLength(200)]
    public string Title { get; set; }

    [Required]
    [StringLength(4000)]
    public string Body { get; set; }

    public int AuthorId { get; set; }
}