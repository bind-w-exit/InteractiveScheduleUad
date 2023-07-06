using InteractiveScheduleUad.Api.Models;
using InteractiveScheduleUad.Api.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace InteractiveScheduleUad.Api.Repositories;

public class UserRepository : RepositoryBase<User>, IUserRepository
{
    public UserRepository(DbContext context) : base(context)
    {
    }
}