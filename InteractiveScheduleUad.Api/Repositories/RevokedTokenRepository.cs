using InteractiveScheduleUad.Api.Models;
using InteractiveScheduleUad.Api.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace InteractiveScheduleUad.Api.Repositories;

public class RevokedTokenRepository : RepositoryBase<RevokedToken>, IRevokedTokenRepository
{
    public RevokedTokenRepository(DbContext context) : base(context)
    {
    }
}