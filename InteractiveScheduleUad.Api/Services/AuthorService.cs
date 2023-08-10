using InteractiveScheduleUad.Api.Mappers;
using InteractiveScheduleUad.Api.Models;
using InteractiveScheduleUad.Api.Models.Dtos;
using InteractiveScheduleUad.Api.Repositories.Contracts;
using InteractiveScheduleUad.Api.Services.Contracts;

namespace InteractiveScheduleUad.Api.Services;

public class AuthorService : IAuthorService
{
    private readonly IAuthorRepository _authorRepository;

    public AuthorService(IAuthorRepository authorRepository)
    {
        _authorRepository = authorRepository;
    }

    public async Task<Author> CreateAsync(AuthorForWriteDto authorForWriteDto)
    {
        Author author = AuthorMapper.AuthorForWriteDtoToAuthor(authorForWriteDto);

        await _authorRepository.InsertAsync(author);
        await _authorRepository.SaveChangesAsync();

        return author;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var author = await _authorRepository.GetByIdAsync(id);
        if (author is not null)
        {
            _authorRepository.Delete(author);
            await _authorRepository.SaveChangesAsync();
            return true;
        }
        return false;
    }

    public async Task<IEnumerable<AuthorForReadDto>> GetAllAsync()
    {
        var authorsFromDb = await _authorRepository.GetAllAsync();
        return authorsFromDb.Select(AuthorMapper.AuthorToAuthorForReadDto);
    }

    public async Task<Author?> GetByIdAsync(int id)
    {
        return await _authorRepository.GetByIdAsync(id);
    }

    public async Task<bool> UpdateAsync(int id, AuthorForWriteDto authorForWriteDto)
    {
        var authorFromDb = await _authorRepository.GetByIdAsync(id);
        if (authorFromDb is not null)
        {
            AuthorMapper.AuthorForWriteDtoToAuthor(authorForWriteDto, authorFromDb);

            _authorRepository.Update(authorFromDb);
            await _authorRepository.SaveChangesAsync();

            return true;
        }
        return false;
    }
}