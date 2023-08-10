using InteractiveScheduleUad.Api.Models;
using InteractiveScheduleUad.Api.Models.Dtos;
using Riok.Mapperly.Abstractions;

namespace InteractiveScheduleUad.Api.Mappers;

[Mapper]
public static partial class AuthorMapper
{
    [MapperIgnoreTarget(nameof(Author.Id))]
    public static partial Author AuthorForWriteDtoToAuthor(AuthorForWriteDto authorForWriteDto);

    [MapperIgnoreTarget(nameof(Author.Id))]
    public static partial void AuthorForWriteDtoToAuthor(AuthorForWriteDto authorForWriteDto, Author author);

    [MapperIgnoreSource(nameof(Author.Bio))]
    public static partial AuthorForReadDto AuthorToAuthorForReadDto(Author author);
}