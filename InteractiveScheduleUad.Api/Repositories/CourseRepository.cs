using InteractiveScheduleUad.Api.Models;
using InteractiveScheduleUad.Api.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace InteractiveScheduleUad.Api.Repositories;

public class CourseRepository : RepositoryBase<Course>, ICourseRepository
{
    public CourseRepository(DbContext context) : base(context)
    {
    }
}