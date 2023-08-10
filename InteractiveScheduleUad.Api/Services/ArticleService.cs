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

    public async Task<Article> CreateAsync(ArticleForWriteDto articleForWriteDto)
    {
        Article article = ArticleMapper.ArticleForWriteDtoToArticle(articleForWriteDto);
        article.Published = DateTime.UtcNow;

        await _articleRepository.InsertAsync(article);
        await _articleRepository.SaveChangesAsync();

        return article;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var article = await _articleRepository.GetByIdAsync(id);
        if (article is not null)
        {
            _articleRepository.Delete(article);
            await _articleRepository.SaveChangesAsync();
            return true;
        }
        return false;
    }

    public async Task<IEnumerable<ArticleForReadDto>> GetAllAsync()
    {
        var articlesFromDb = await _articleRepository.GetAllAsync();
        return articlesFromDb.Select(ArticleMapper.ArticleToArticleForReadDto);
    }

    public async Task<Article?> GetByIdAsync(int id)
    {
        return await _articleRepository.GetByIdAsync(id);
    }

    public async Task<bool> UpdateAsync(int id, ArticleForWriteDto articleForWriteDto)
    {
        var articlesFromDb = await _articleRepository.GetByIdAsync(id);
        if (articlesFromDb is not null)
        {
            ArticleMapper.ArticleForWriteDtoToArticle(articleForWriteDto, articlesFromDb);

            _articleRepository.Update(articlesFromDb);
            await _articleRepository.SaveChangesAsync();

            return true;
        }
        return false;
    }
}