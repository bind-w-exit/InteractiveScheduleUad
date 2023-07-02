using InteractiveScheduleUad.Api.Models;
using InteractiveScheduleUad.Api.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace InteractiveScheduleUad.Api.Repositories;

public class RoomRepository : RepositoryBase<Room>, IRoomRepository
{
    public RoomRepository(DbContext context) : base(context)
    {
    }
}