using InteractiveScheduleUad.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace InteractiveScheduleUad.Api;

// The database context is the main class that coordinates Entity Framework functionality for a data model.
// note: does not contain schedule DbSet as it is saved under StudentsGroup
public class InteractiveScheduleUadApiDbContext : DbContext
{
    public InteractiveScheduleUadApiDbContext(DbContextOptions<InteractiveScheduleUadApiDbContext> options)
    : base(options)
    {
    }

    // schedules
    public DbSet<StudentsGroup> StudentsGroups { get; set; }

    //public DbSet<WeekSchedule> Schedules { get; set; }

    public DbSet<Room> Rooms { get; set; }

    public DbSet<Subject> Subjects { get; set; }

    public DbSet<Lesson> Lessons { get; set; }

    // schedules [new stuff]

    public DbSet<TimeContext> TimeContexts { get; set; }

    public DbSet<ScheduleLesson> ScheduleLessons { get; set; }

    // teacher
    public DbSet<Teacher> Teachers { get; set; }

    public DbSet<Department> Departments { get; set; }

    // authorization
    public DbSet<User> Users { get; set; }

    public DbSet<RevokedToken> RevokedTokens { get; set; }

    // news
    public DbSet<Article> Articles { get; set; }

    public DbSet<Author> Authors { get; set; }
}