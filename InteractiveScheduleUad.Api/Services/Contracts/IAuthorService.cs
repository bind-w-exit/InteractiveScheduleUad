using FluentResults;
using InteractiveScheduleUad.Api.Models;
using InteractiveScheduleUad.Api.Models.Dtos;

namespace InteractiveScheduleUad.Api.Services.Contracts;

public interface IAuthorService
{
    Task<Result<Author>> CreateAsync(NewsAuthorForWriteDto authorForWriteDto);

    Task<Result<IEnumerable<NewsAuthorForReadDto>>> GetAllAsync();

    Task<Result<Author>> GetByIdAsync(int id);

    Task<Result> UpdateAsync(int id, NewsAuthorForWriteDto authorForWriteDto);

    Task<Result> DeleteAsync(int id);
}