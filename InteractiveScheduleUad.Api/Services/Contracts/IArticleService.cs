using FluentResults;
using InteractiveScheduleUad.Api.Models;
using InteractiveScheduleUad.Api.Models.Dtos;

namespace InteractiveScheduleUad.Api.Services.Contracts;

public interface IArticleService
{
    Task<Result<Article>> CreateAsync(NewsArticleForWriteDto articleForWriteDto);

    Task<Result<IEnumerable<NewsArticleForReadDto>>> GetAllAsync();

    Task<Result<Article>> GetByIdAsync(int id);

    Task<Result> UpdateAsync(int id, NewsArticleForWriteDto articleForWriteDto);

    Task<Result> DeleteAsync(int id);
}