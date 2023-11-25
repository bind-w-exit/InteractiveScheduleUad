namespace InteractiveScheduleUad.Api.Models.Dtos;

// TODO: Annotate
public class NewsArticleForReadDto
{
    public int Id { get; set; }

    public string Title { get; set; }

    public DateTime Published { get; set; }

    public int AuthorId { get; set; }
}