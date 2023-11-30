using FluentValidation;
using InteractiveScheduleUad.Api;
using InteractiveScheduleUad.Api.Middleware;
using InteractiveScheduleUad.Api.Models;
using InteractiveScheduleUad.Api.Repositories;
using InteractiveScheduleUad.Api.Repositories.Contracts;
using InteractiveScheduleUad.Api.Services;
using InteractiveScheduleUad.Api.Services.AuthAndAuthorizationServices;
using InteractiveScheduleUad.Api.Services.AuthAndAuthorizationServices.Contracts;
using InteractiveScheduleUad.Api.Services.Contracts;
using InteractiveScheduleUad.Api.Validators;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Npgsql;
using Swashbuckle.AspNetCore.Filters;
using System.Data;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;

// is necessary for in-memory DB scenario to work locally, on windows
DotNetEnv.Env.TraversePath().Load();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers(options =>
{
    // TODO: Will this add to performance? Because then FluentValidation will check this again
    options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
}).AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

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
    // note: generation of documentation file has to be enabled in project properties:
    // Project properties -> Build -> Output -> Documentation file
});

// Database

// the SHOULD_USE_IN_MEMORY_DB is set by one of launch profiles.
bool shouldUseInMemoryDb = builder.Configuration["SHOULD_USE_IN_MEMORY_DB"] == "true";
// the SHOULD_USE_NONCONTAINERIZED_LOCAL_DB is set by one of launch profiles.
bool shouldUseNonContainerizedLocalDb = builder.Configuration["SHOULD_USE_NONCONTAINERIZED_LOCAL_DB"] == "true";

if (shouldUseInMemoryDb)
{
    // add database (in-memory)
    Console.WriteLine("Using in-memory database");
    builder.Services.AddDbContext<DbContext, InteractiveScheduleUadApiDbContext>(options =>
           options
           .UseInMemoryDatabase("SchedulesDB")
           .UseLazyLoadingProxies()
           );
}
else
{
    string connectionString;

    if (shouldUseNonContainerizedLocalDb)
    {
        connectionString = builder.Configuration["NONCONTAINERIZED_LOCAL_DB_CONNECTION_STRING"];
    }
    else
    {
        connectionString = GetDbConnectionString(builder.Configuration);
    }

    //connectionString = "Host=localhost;Database=realSCDB;Username=postgres;Password=1;IncludeErrorDetail=true";
    //var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

    // connect to npgsql db. Exit on failure
    if (CheckNpgsqlDbConnection(connectionString))
    {
        builder.Services.AddDbContext<DbContext, InteractiveScheduleUadApiDbContext>(options =>
            {
                options.UseNpgsql(connectionString);
                options.EnableSensitiveDataLogging();
                options.UseLazyLoadingProxies();
            }
            );
    }
    else
    {
        Console.WriteLine("Failed to connect to the database after multiple attempts. Exiting...");
        Environment.Exit(1);
    }
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
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
            ClockSkew = TimeSpan.FromSeconds(10)
        };
    });

// Authorization
AddAuthorizationPolicies(builder.Services);

// Validation (for rooms only, for now)
builder.Services.AddValidatorsFromAssemblyContaining<RoomForWriteDtoValidator>();

// add transient services for repositories. Transient services are created each time they are requested
// TODO: Change to AddScoped. Scoped services are created once per scope
builder.Services.AddTransient<ISubjectRepository, SubjectRepository>();
builder.Services.AddTransient<IDepartmentRepository, DepartmentRepository>();
builder.Services.AddTransient<IStudentsGroupRepository, StudentsGroupRepository>();
builder.Services.AddTransient<ITeacherRepository, TeacherRepository>();
builder.Services.AddTransient<IRoomRepository, RoomRepository>();
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IRevokedTokenRepository, RevokedTokenRepository>();
builder.Services.AddTransient<IAuthorRepository, AuthorRepository>();
builder.Services.AddTransient<IArticleRepository, ArticleRepository>();
builder.Services.AddTransient<IWeekScheduleRepository, WeekScheduleRepository>();

// add transient services. Those build on top of repositories
builder.Services.AddTransient<IDepartmentService, DepartmentService>();
builder.Services.AddTransient<IStudentsGroupService, StudentsGroupService>();
builder.Services.AddTransient<IRoomService, RoomService>();
builder.Services.AddTransient<ISubjectService, SubjectService>();
builder.Services.AddTransient<ITeacherService, TeacherService>();
builder.Services.AddTransient<IAuthService, AuthService>();
builder.Services.AddTransient<IAuthorService, AuthorService>();
builder.Services.AddTransient<IArticleService, ArticleService>();

// add auth related services
builder.Services.AddTransient<IHashService, HashService>();
builder.Services.AddTransient<ITokenService, TokenService>();

var app = builder.Build();

// not sure if it works
app.UseMiddleware<ErrorLoggingMiddleware>();

// expose Content-Range header for react-admin
app.UseMiddleware<HeadersMiddleware>();

// enable cross-origin requests
app.UseCors(builder => builder
           .AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader());

using var scope = app.Services.CreateScope();

app.UseSwagger();
app.UseSwaggerUI();

// do not use migrations for in-memory db
if (!shouldUseInMemoryDb)
{
    // about migrations:
    // https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/?tabs=dotnet-core-cli
    var apiContext = scope.ServiceProvider.GetRequiredService<InteractiveScheduleUadApiDbContext>();
    apiContext.Database.Migrate();
}

var authService = scope.ServiceProvider.GetRequiredService<IAuthService>();
var userRepositoryService = scope.ServiceProvider.GetRequiredService<IUserRepository>();
await CreateFirstUserIfEmpty(authService, userRepositoryService, app.Configuration);

app.UseHttpsRedirection();

app.UseAuthorization();

// maps annotated controllers to routes, I guess?
app.MapControllers();

app.Run();

// extracts connection string bits from configuration (.env file, container in compose scenario) and returns them stringed together
static string GetDbConnectionString(IConfiguration configuration)
{
    var host = configuration["DATABASE_HOST"];
    var database = configuration["DATABASE_NAME"];
    var username = configuration["DATABASE_USER"];
    var password = configuration["DATABASE_PASSWORD"];
    return $"Host={host};Database={database};Username={username};Password={password}"; ;
}

static bool CheckNpgsqlDbConnection(string connectionString)
{
    int maxAttempts = 10;
    int currentAttempt = 0;
    TimeSpan delay = TimeSpan.FromSeconds(3);

    while (currentAttempt < maxAttempts)
    {
        try
        {
            Console.WriteLine($"Using this conn string: {connectionString}");
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
            string roleStr = role.ToString();
            options.AddPolicy(roleStr, policy =>
                policy.RequireRole(roleStr));
        });
    }

    services.AddAuthorization(options =>
    {
        options.AddPolicy("RefreshToken", policy =>
                policy.RequireRole("RefreshToken"));
    });
}

// creates admin user in case none exists
static async Task CreateFirstUserIfEmpty(IAuthService authService, IUserRepository userRepository, IConfiguration configuration)
{
    var admin = await userRepository.FirstOrDefaultAsync(user => user.UserRole == UserRole.Admin);
    if (admin is null)
    {
        var adminUsername = configuration["ADMIN_USERNAME"];
        var adminPassword = configuration["ADMIN_PASSWORD"];

        await authService.Register(new()
        {
            Username = adminUsername,
            Password = adminPassword,
            UserRole = UserRole.Admin
        });
    }
}