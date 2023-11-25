using InteractiveScheduleUad.Api.Models;
using InteractiveScheduleUad.Api.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace InteractiveScheduleUad.Api.Repositories;

public class WeekScheduleRepository : RepositoryBase<WeekSchedule_DoNotUse>, IWeekScheduleRepository
{
    public WeekScheduleRepository(DbContext context) : base(context)
    {
    }
}