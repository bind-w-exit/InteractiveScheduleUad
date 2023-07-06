using InteractiveScheduleUad.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace InteractiveScheduleUad.Api;

public class InteractiveScheduleUadApiDbContext : DbContext
{
    public InteractiveScheduleUadApiDbContext(DbContextOptions<InteractiveScheduleUadApiDbContext> options)
    : base(options)
    {
    }

    public DbSet<StudentsGroup> StudentsGroups { get; set; }

    public DbSet<Teacher> Teachers { get; set; }

    public DbSet<Subject> Subjects { get; set; }

    public DbSet<Department> Departments { get; set; }

    public DbSet<Room> Rooms { get; set; }

    public DbSet<User> Users { get; set; }

    public DbSet<RevokedToken> RevokedTokens { get; set; }
}