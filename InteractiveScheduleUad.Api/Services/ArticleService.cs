using FluentResults;
using InteractiveScheduleUad.Api.Errors;
using InteractiveScheduleUad.Api.Mappers;
using InteractiveScheduleUad.Api.Models;
using InteractiveScheduleUad.Api.Models.Dtos;
using InteractiveScheduleUad.Api.Repositories.Contracts;
using InteractiveScheduleUad.Api.Services.Contracts;

namespace InteractiveScheduleUad.Api.Services;

public class ArticleService : IArticleService
{
    private readonly IArticleRepository _articleRepository;

    public ArticleService(IArticleRepository articleRepository)
    {
        _articleRepository = articleRepository;
    }

    public async Task<Result<Article>> CreateAsync(NewsArticleForWriteDto articleForWriteDto)
    {
        Article article = ArticleMapper.ArticleForWriteDtoToArticle(articleForWriteDto);
        article.Published = DateTime.UtcNow;

        await _articleRepository.InsertAsync(article);
        await _articleRepository.SaveChangesAsync();

        return article;
    }

    public async Task<Result> DeleteAsync(int id)
    {
        var article = await _articleRepository.GetByIdAsync(id);

        if (article is not null)
        {
            _articleRepository.Delete(article);
            await _articleRepository.SaveChangesAsync();

            return Result.Ok();
        }
        else
            return new NotFoundError(nameof(Article));
    }

    public async Task<Result<IEnumerable<NewsArticleForReadDto>>> GetAllAsync()
    {
        var articles = await _articleRepository.GetAllAsync();
        var mappedArticles = articles.Select(ArticleMapper.ArticleToArticleForReadDto);

        return Result.Ok();
    }

    public async Task<Result<Article>> GetByIdAsync(int id)
    {
        var article = await _articleRepository.GetByIdAsync(id);

        if (article is not null)
            return article;
        else
            return new NotFoundError(nameof(Article));
    }

    public async Task<Result> UpdateAsync(int id, NewsArticleForWriteDto articleForWriteDto)
    {
        var articles = await _articleRepository.GetByIdAsync(id);

        if (articles is not null)
        {
            ArticleMapper.ArticleForWriteDtoToArticle(articleForWriteDto, articles);

            _articleRepository.Update(articles);
            await _articleRepository.SaveChangesAsync();

            return Result.Ok();
        }
        else
            return new NotFoundError(nameof(Article));
    }
}