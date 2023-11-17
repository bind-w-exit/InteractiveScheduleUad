using InteractiveScheduleUad.Api.Models;
using InteractiveScheduleUad.Api.Models.Dtos;
using Riok.Mapperly.Abstractions;

namespace InteractiveScheduleUad.Api.Mappers;

[Mapper]
public static partial class AuthorMapper
{
    [MapperIgnoreTarget(nameof(Author.Id))]
    public static partial Author AuthorForWriteDtoToAuthor(NewsAuthorForWriteDto authorForWriteDto);

    [MapperIgnoreTarget(nameof(Author.Id))]
    public static partial void AuthorForWriteDtoToAuthor(NewsAuthorForWriteDto authorForWriteDto, Author author);

    [MapperIgnoreSource(nameof(Author.Bio))]
    public static partial NewsAuthorForReadDto AuthorToAuthorForReadDto(Author author);
}