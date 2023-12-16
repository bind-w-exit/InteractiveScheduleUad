using InteractiveScheduleUad.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace InteractiveScheduleUad.Api;

// The database context is the main class that coordinates Entity Framework functionality for a data model.
public class InteractiveScheduleUadApiDbContext : DbContext
{
    public InteractiveScheduleUadApiDbContext(DbContextOptions<InteractiveScheduleUadApiDbContext> options)
    : base(options)
    {
    }

    // schedules

    public DbSet<StudentsGroup> StudentsGroups { get; set; }

    public DbSet<Room> Rooms { get; set; }

    public DbSet<Subject> Subjects { get; set; }

    public DbSet<Lesson> Lessons { get; set; }
    public DbSet<ScheduleLessonJunction> ScheduleLessonJunctions { get; set; }

    // contexts

    public DbSet<TimeContext> TimeContexts { get; set; }

    public DbSet<FullContextJunction> FullContexts { get; set; }

    // teacher

    public DbSet<Teacher> Teachers { get; set; }

    public DbSet<Department> Departments { get; set; }

    // authentication

    public DbSet<User> Users { get; set; }

    public DbSet<RevokedToken> RevokedTokens { get; set; }

    // news

    public DbSet<Article> Articles { get; set; }

    public DbSet<Author> Authors { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // configure not cascade deleting Teacher and Room when Lesson is deleted

        modelBuilder.Entity<Lesson>()
            .HasOne(nameof(Teacher))
            .WithMany(nameof(Lesson))
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Lesson>()
            .HasOne(nameof(Room))
            .WithMany(nameof(Lesson))
            .OnDelete(DeleteBehavior.SetNull);

        // create unique index for lesson and make nulls not distinct (in postgresql, nulls are distinct by default)

        modelBuilder.Entity<Lesson>()
            .HasIndex(l => new { l.ClassType, l.SubjectId, l.TeacherId, l.RoomId })
            .IsUnique()
            .AreNullsDistinct(false);

        base.OnModelCreating(modelBuilder);
    }
}