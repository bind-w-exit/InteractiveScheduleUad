using InteractiveScheduleUad.Api.Models;
using InteractiveScheduleUad.Api.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace InteractiveScheduleUad.Api.Repositories;

public class ArticleRepository : RepositoryBase<Article>, IArticleRepository
{
    public ArticleRepository(DbContext context) : base(context)
    {
    }
}