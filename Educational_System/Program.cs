using EducationalSystem.BLL.Repositories.Interfaces;
using EducationalSystem.BLL.Repositories.Repositories;
using EducationalSystem.DAL.Models;
using EducationalSystem.DAL.Models.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace EducationalSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();

            // Add DbContext with the MigrationsAssembly
            builder.Services.AddDbContext<Education_System>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DB1"),
                    b => b.MigrationsAssembly("EducationalSystem.DAL")));  // Specify the migrations assembly here

            // Register Identity services
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                //options.SignIn.RequireConfirmedAccount = true;
                // Password settings.
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 0;

                // User settings.
                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
            })
                .AddEntityFrameworkStores<Education_System>()
                .AddDefaultTokenProviders();  // Registers the UserManager and other Identity-related services

            // Add scoped repositories for dependency injection

            //scoped repositories Clean
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

            //builder.Services.AddScoped<IGenericRepository<Instructors>, GenericReposiory<Instructors>>();
            //builder.Services.AddScoped<IGenericRepository<Courses>, GenericReposiory<Courses>>();
            //builder.Services.AddScoped<IGenericRepository<Course_Instructors>, GenericReposiory<Course_Instructors>>();

            // Add Swagger for API documentation
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
