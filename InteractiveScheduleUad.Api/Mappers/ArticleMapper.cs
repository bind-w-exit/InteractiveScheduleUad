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
    public static partial Article ArticleForWriteDtoToArticle(ArticleForWriteDto articleForWriteDto);

    [MapperIgnoreTarget(nameof(Article.Id))]
    [MapperIgnoreTarget(nameof(Article.Author))]
    [MapperIgnoreTarget(nameof(Article.Published))]
    public static partial void ArticleForWriteDtoToArticle(ArticleForWriteDto articleForWriteDto, Article article);

    [MapperIgnoreSource(nameof(Article.Body))]
    [MapperIgnoreSource(nameof(Article.Author))]
    public static partial ArticleForReadDto ArticleToArticleForReadDto(Article article);
}