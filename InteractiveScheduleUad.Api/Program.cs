using InteractiveScheduleUad.Api;
using InteractiveScheduleUad.Api.Repositories;
using InteractiveScheduleUad.Api.Repositories.Contracts;
using InteractiveScheduleUad.Api.Services;
using InteractiveScheduleUad.Api.Services.Contracts;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database
var connectionString = GetDbConnectionString(builder.Configuration);
builder.Services.AddDbContext<DbContext, InteractiveScheduleUadApiDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddTransient<ICourseRepository, CourseRepository>();
builder.Services.AddTransient<IDepartmentRepository, DepartmentRepository>();
builder.Services.AddTransient<IStudentsGroupRepository, StudentsGroupRepository>();
builder.Services.AddTransient<ITeacherRepository, TeacherRepository>();
builder.Services.AddTransient<IRoomRepository, RoomRepository>();

builder.Services.AddTransient<IDepartmentService, DepartmentService>();
builder.Services.AddTransient<IStudentsGroupService, StudentsGroupService>();
builder.Services.AddTransient<IWeekScheduleService, WeekScheduleService>();
builder.Services.AddTransient<IRoomService, RoomService>();
builder.Services.AddTransient<ICourseService, CourseService>();
builder.Services.AddTransient<ITeacherService, TeacherService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    using var scope = app.Services.CreateScope();
    var apiContext = scope.ServiceProvider.GetRequiredService<InteractiveScheduleUadApiDbContext>();
    apiContext.Database.EnsureDeleted();
    apiContext.Database.EnsureCreated();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

static string GetDbConnectionString(IConfiguration configuration)
{
    var host = configuration["DB_HOST"];
    var database = configuration["DB_NAME"];
    var username = configuration["DB_USER"];
    var password = configuration["DB_PASS"];
    return $"Host={host};Database={database};Username={username};Password={password}";
}