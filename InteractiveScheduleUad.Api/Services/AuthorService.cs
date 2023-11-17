using FluentResults;
using InteractiveScheduleUad.Api.Errors;
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

    public async Task<Result<Author>> CreateAsync(NewsAuthorForWriteDto authorForWriteDto)
    {
        Author author = AuthorMapper.AuthorForWriteDtoToAuthor(authorForWriteDto);

        await _authorRepository.InsertAsync(author);
        await _authorRepository.SaveChangesAsync();

        return author;
    }

    public async Task<Result> DeleteAsync(int id)
    {
        var author = await _authorRepository.GetByIdAsync(id);

        if (author is not null)
        {
            _authorRepository.Delete(author);
            await _authorRepository.SaveChangesAsync();

            return Result.Ok();
        }
        else
            return new NotFoundError(nameof(Author));
    }

    public async Task<Result<IEnumerable<NewsAuthorForReadDto>>> GetAllAsync()
    {
        var authors = await _authorRepository.GetAllAsync();
        var mappedAuthors = authors.Select(AuthorMapper.AuthorToAuthorForReadDto);

        return Result.Ok(mappedAuthors);
    }

    public async Task<Result<Author>> GetByIdAsync(int id)
    {
        var author = await _authorRepository.GetByIdAsync(id);

        if (author is not null)
            return author;
        else
            return new NotFoundError(nameof(Author));
    }

    public async Task<Result> UpdateAsync(int id, NewsAuthorForWriteDto authorForWriteDto)
    {
        var author = await _authorRepository.GetByIdAsync(id);

        if (author is not null)
        {
            AuthorMapper.AuthorForWriteDtoToAuthor(authorForWriteDto, author);

            _authorRepository.Update(author);
            await _authorRepository.SaveChangesAsync();

            return Result.Ok();
        }
        else
            return new NotFoundError(nameof(Author));
    }
}