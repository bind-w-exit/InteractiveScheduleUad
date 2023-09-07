using FluentResults;
using InteractiveScheduleUad.Api.Models;
using InteractiveScheduleUad.Api.Models.Dtos;

namespace InteractiveScheduleUad.Api.Services.Contracts;

public interface IAuthorService
{
    Task<Result<Author>> CreateAsync(AuthorForWriteDto authorForWriteDto);

    Task<Result<IEnumerable<AuthorForReadDto>>> GetAllAsync();

    Task<Result<Author>> GetByIdAsync(int id);

    Task<Result> UpdateAsync(int id, AuthorForWriteDto authorForWriteDto);

    Task<Result> DeleteAsync(int id);
}