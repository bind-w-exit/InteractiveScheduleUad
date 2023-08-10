using System.ComponentModel.DataAnnotations;

namespace InteractiveScheduleUad.Api.Models;

public class Article
{
    public int Id { get; set; }

    [Required]
    [StringLength(200)]
    public string Title { get; set; }

    [Required]
    [StringLength(4000)]
    public string Body { get; set; }

    public DateTime Published { get; set; }

    public int AuthorId { get; set; }

    public Author Author { get; set; }
}