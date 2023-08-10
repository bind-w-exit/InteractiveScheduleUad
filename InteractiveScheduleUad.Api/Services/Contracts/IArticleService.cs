using InteractiveScheduleUad.Api.Models;
using InteractiveScheduleUad.Api.Models.Dtos;

namespace InteractiveScheduleUad.Api.Services.Contracts;

public interface IArticleService
{
    Task<Article> CreateAsync(ArticleForWriteDto articleForWriteDto);

    Task<IEnumerable<ArticleForReadDto>> GetAllAsync();

    Task<Article?> GetByIdAsync(int id);

    Task<bool> UpdateAsync(int id, ArticleForWriteDto articleForWriteDto);

    Task<bool> DeleteAsync(int id);
}