using Educational_System.Helpers;
using EducationalSystem.BLL;
using EducationalSystem.BLL.Repositories.Interfaces;
using EducationalSystem.BLL.Repositories.Repositories;
using EducationalSystem.DAL.Models;
using EducationalSystem.DAL.Models.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Reflection;

namespace EducationalSystem
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var logFileName = $"Logs/myapp_{DateTime.Now:yyyyMMdd_HHmmss}.txt";

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Console()
                .WriteTo.Debug()
                .WriteTo.File(logFileName)
                .CreateLogger();

            try
            {
                Log.Information("Starting up the application...");

                var builder = WebApplication.CreateBuilder(args);

                // Replace the default logger with Serilog
                builder.Host.UseSerilog();

                // Add services to the container.
                builder.Services.AddControllers();
                builder.Services.AddScoped<EmailService>();

                // Auto Mapper
                builder.Services.AddAutoMapper(M => M.AddProfile(new MappingProfiles()));

                // DbContext with migration assembly
                builder.Services.AddDbContext<Education_System>(options =>
                    options.UseSqlServer(builder.Configuration.GetConnectionString("DB1"),
                        b => b.MigrationsAssembly("EducationalSystem.DAL")));

                // Caching
                builder.Services.AddMemoryCache();
                builder.Services.AddStackExchangeRedisCache(options =>
                {
                    options.Configuration = "localhost:6379"; // Assuming Redis is running locally
                });

                // Identity config
                builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
                {
                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequiredLength = 6;
                    options.Password.RequiredUniqueChars = 0;
                    options.User.AllowedUserNameCharacters =
                        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                })
                .AddEntityFrameworkStores<Education_System>()
                .AddDefaultTokenProviders();

                // Register generic repositories
                var entityTypes = Assembly.GetAssembly(typeof(BaseEntity))
                    .GetTypes()
                    .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(BaseEntity)))
                    .ToList();

                foreach (var entityType in entityTypes)
                {
                    var interfaceType = typeof(IGenericRepository<>).MakeGenericType(entityType);
                    var implementationType = typeof(GenericReposiory<>).MakeGenericType(entityType);
                    builder.Services.AddScoped(interfaceType, implementationType);
                }

                builder.Services.AddScoped<IInstructorRepository, InstructorRepository>();
                builder.Services.AddScoped<ICourseRepository, CourseRepository>();
                builder.Services.AddScoped<ILessonRepository, LessonRepository>();
                builder.Services.AddScoped<ISubLessonRepository, SubLessonRepository>();
                builder.Services.AddScoped<IDiscountRepository, DiscountRepository>();

                // Enable CORS
                builder.Services.AddCors(options =>
                {
                    options.AddPolicy("AllowAll", policy =>
                    {
                        policy.AllowAnyOrigin()
                              .AllowAnyHeader()
                              .AllowAnyMethod();
                    });
                });

                // Swagger
                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddSwaggerGen();

                var app = builder.Build();

                // Serve static files from wwwroot
                app.UseStaticFiles();

                // Database seeding
                using (var scope = app.Services.CreateScope())
                {
                    var services = scope.ServiceProvider;
                    await RunSeedingAsync(services);
                }

                // Configure pipeline
                if (app.Environment.IsDevelopment())
                {
                    app.UseSwagger();
                    app.UseSwaggerUI();
                }

                app.UseCors("AllowAll");
                app.UseAuthorization();
                app.MapControllers();

                await app.RunAsync();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application terminated unexpectedly!");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private static async Task RunSeedingAsync(IServiceProvider services)
        {
            var loggerFactory = services.GetRequiredService<ILoggerFactory>();
            var logger = loggerFactory.CreateLogger<Program>();

            try
            {
                var dbContext = services.GetRequiredService<Education_System>();
                var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

                await Education_System_Seeding.SeedUsersAndRolesAsync(userManager, roleManager);
                await Education_System_Seeding.SeedAsync(dbContext);

                logger.LogInformation("Database migration and seeding completed.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error during database seeding: {Message}", ex.Message);
                throw;
            }
        }
    }
}
