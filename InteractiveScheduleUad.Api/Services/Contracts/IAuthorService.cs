using InteractiveScheduleUad.Api.Models;
using InteractiveScheduleUad.Api.Models.Dtos;

namespace InteractiveScheduleUad.Api.Services.Contracts;

public interface IAuthorService
{
    Task<Author> CreateAsync(AuthorForWriteDto authorForWriteDto);

    Task<IEnumerable<AuthorForReadDto>> GetAllAsync();

    Task<Author?> GetByIdAsync(int id);

    Task<bool> UpdateAsync(int id, AuthorForWriteDto authorForWriteDto);

    Task<bool> DeleteAsync(int id);
}