using InteractiveScheduleUad.Api.Models;
using InteractiveScheduleUad.Api.Models.Dtos;
using Riok.Mapperly.Abstractions;

namespace InteractiveScheduleUad.Api.Mappers;

[Mapper]
public static partial class ArticleMapper
{
    [MapperIgnoreTarget(nameof(Article.Id))]
    [MapperIgnoreTarget(nameof(Article.Author))]
    [MapperIgnoreTarget(nameof(Article.Published))]
    public static partial Article ArticleForWriteDtoToArticle(NewsArticleForWriteDto articleForWriteDto);

    [MapperIgnoreTarget(nameof(Article.Id))]
    [MapperIgnoreTarget(nameof(Article.Author))]
    [MapperIgnoreTarget(nameof(Article.Published))]
    public static partial void ArticleForWriteDtoToArticle(NewsArticleForWriteDto articleForWriteDto, Article article);

    [MapperIgnoreSource(nameof(Article.Body))]
    [MapperIgnoreSource(nameof(Article.Author))]
    public static partial NewsArticleForReadDto ArticleToArticleForReadDto(Article article);
}