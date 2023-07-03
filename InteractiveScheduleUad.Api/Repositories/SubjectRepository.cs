using InteractiveScheduleUad.Api.Models;
using InteractiveScheduleUad.Api.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace InteractiveScheduleUad.Api.Repositories;

public class SubjectRepository : RepositoryBase<Subject>, ISubjectRepository
{
    public SubjectRepository(DbContext context) : base(context)
    {
    }
}