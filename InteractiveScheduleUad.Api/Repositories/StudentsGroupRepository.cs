using InteractiveScheduleUad.Api.Models;
using InteractiveScheduleUad.Api.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace InteractiveScheduleUad.Api.Repositories;

public class StudentsGroupRepository : RepositoryBase<StudentsGroup>, IStudentsGroupRepository
{
    public StudentsGroupRepository(DbContext context) : base(context)
    {
    }
}