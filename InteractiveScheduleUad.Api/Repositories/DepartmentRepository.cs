using InteractiveScheduleUad.Api.Models;
using InteractiveScheduleUad.Api.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace InteractiveScheduleUad.Api.Repositories;

public class DepartmentRepository : RepositoryBase<Department>, IDepartmentRepository
{
    public DepartmentRepository(DbContext context) : base(context)
    {
    }
}