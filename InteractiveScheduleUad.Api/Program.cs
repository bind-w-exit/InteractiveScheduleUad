using InteractiveScheduleUad.Api;
using InteractiveScheduleUad.Api.Models;
using InteractiveScheduleUad.Api.Repositories;
using InteractiveScheduleUad.Api.Repositories.Contracts;
using InteractiveScheduleUad.Api.Services;
using InteractiveScheduleUad.Api.Services.Contracts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Npgsql;
using Swashbuckle.AspNetCore.Filters;
using System.Data;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Interactive Schedule UAD API",
        Version = "v1",
        Contact = new OpenApiContact
        {
            Name = "GitHub",
            Url = new Uri("https://github.com/bind-w-exit/InteractiveScheduleUad")
        },
        License = new OpenApiLicense()
        {
            Name = "MIT",
            Url = new Uri("https://opensource.org/licenses/MIT")
        }
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JSON Web Token based security"
    });

    options.OperationFilter<SecurityRequirementsOperationFilter>(true, "Bearer");

    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

// Database
var connectionString = GetDbConnectionString(builder.Configuration);

if (CheckDbConnection(connectionString))
{
    builder.Services.AddDbContext<DbContext, InteractiveScheduleUadApiDbContext>(options =>
        options.UseNpgsql(connectionString));
}
else
{
    Console.WriteLine("Failed to connect to the database after multiple attempts. Exiting...");
    Environment.Exit(1);
}

// Authentication
var issuer = builder.Configuration["JWT_ISSUER"];
var audience = builder.Configuration["JWT_AUDIENCE"];
var key = builder.Configuration["JWT_ACCESS_TOKEN_SECRET"];
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = issuer,
            ValidAudience = audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
        };
    });

// Authorization
AddAuthorizationPolicies(builder.Services);

builder.Services.AddTransient<ISubjectRepository, SubjectRepository>();
builder.Services.AddTransient<IDepartmentRepository, DepartmentRepository>();
builder.Services.AddTransient<IStudentsGroupRepository, StudentsGroupRepository>();
builder.Services.AddTransient<ITeacherRepository, TeacherRepository>();
builder.Services.AddTransient<IRoomRepository, RoomRepository>();
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IRevokedTokenRepository, RevokedTokenRepository>();

builder.Services.AddTransient<IDepartmentService, DepartmentService>();
builder.Services.AddTransient<IStudentsGroupService, StudentsGroupService>();
builder.Services.AddTransient<IWeekScheduleService, WeekScheduleService>();
builder.Services.AddTransient<IRoomService, RoomService>();
builder.Services.AddTransient<ISubjectService, SubjectService>();
builder.Services.AddTransient<ITeacherService, TeacherService>();
builder.Services.AddTransient<IAuthService, AuthService>();

builder.Services.AddSingleton<IHashService, HashService>();
builder.Services.AddSingleton<ITokenService, TokenService>();

var app = builder.Build();

using var scope = app.Services.CreateScope();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    var apiContext = scope.ServiceProvider.GetRequiredService<InteractiveScheduleUadApiDbContext>();
    apiContext.Database.EnsureDeleted();
    apiContext.Database.EnsureCreated();
}

await CreateFirstUser(scope.ServiceProvider.GetRequiredService<IAuthService>(), scope.ServiceProvider.GetRequiredService<IUserRepository>(), app.Configuration);

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

static bool CheckDbConnection(string connectionString)
{
    int maxAttempts = 10;
    int currentAttempt = 0;
    TimeSpan delay = TimeSpan.FromSeconds(3);

    while (currentAttempt < maxAttempts)
    {
        try
        {
            using NpgsqlConnection connection = new(connectionString);
            connection.Open();

            Console.WriteLine("Connected to the database!");
            return true;
        }
        catch (Exception)
        {
            currentAttempt++;
            Console.WriteLine($"Attempt {currentAttempt}/{maxAttempts}: Failed to connect to the database. Retrying in {delay.TotalSeconds} seconds...");
            Thread.Sleep(delay);
        }
    }

    return false;
}

static void AddAuthorizationPolicies(IServiceCollection services)
{
    var roles = Enum.GetValues(typeof(UserRole)).Cast<UserRole>();
    foreach (var role in roles)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy(role.ToString(), policy =>
                policy.RequireRole(role.ToString()));
        });
    }

    services.AddAuthorization(options =>
    {
        options.AddPolicy("RefreshToken", policy =>
                policy.RequireRole("RefreshToken"));
    });
}

static async Task CreateFirstUser(IAuthService authService, IUserRepository userRepository, IConfiguration configuration)
{
    var admin = await userRepository.SingleOrDefaultAsync(user => user.UserRole == UserRole.Admin);
    if (admin is null)
    {
        authService.Register(new() { Username = configuration["ADMIN_USERNAME"], Password = configuration["ADMIN_PASSWORD"] });
    }
}