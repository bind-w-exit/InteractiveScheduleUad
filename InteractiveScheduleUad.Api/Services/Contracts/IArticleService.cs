using FluentResults;
using InteractiveScheduleUad.Api.Models;
using InteractiveScheduleUad.Api.Models.Dtos;

namespace InteractiveScheduleUad.Api.Services.Contracts;

public interface IArticleService
{
    Task<Result<Article>> CreateAsync(ArticleForWriteDto articleForWriteDto);

    Task<Result<IEnumerable<ArticleForReadDto>>> GetAllAsync();

    Task<Result<Article>> GetByIdAsync(int id);

    Task<Result> UpdateAsync(int id, ArticleForWriteDto articleForWriteDto);

    Task<Result> DeleteAsync(int id);
}