using InteractiveScheduleUad.Api.Models;
using InteractiveScheduleUad.Api.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace InteractiveScheduleUad.Api.Repositories;

public class TeacherRepository : RepositoryBase<Teacher>, ITeacherRepository
{
    public TeacherRepository(DbContext context) : base(context)
    {
    }
}