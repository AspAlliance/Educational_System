using EducationalSystem.BLL.Repositories.Interfaces;
using EducationalSystem.BLL.Repositories.Repositories;
using EducationalSystem.DAL.Models;
using EducationalSystem.DAL.Models.Context;
using Microsoft.EntityFrameworkCore;

namespace EducationalSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Add DbContext with the MigrationsAssembly
            builder.Services.AddDbContext<Education_System>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DB1"),
        b => b.MigrationsAssembly("EducationalSystem.DAL")));  // Specify the migrations assembly here
            builder.Services.AddScoped<IGenericRepository<Instructors>, GenericReposiory<Instructors>>();
            builder.Services.AddScoped<IGenericRepository<Courses>, GenericReposiory<Courses>>();
            builder.Services.AddScoped<IGenericRepository<Course_Instructors>, GenericReposiory<Course_Instructors>>();

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
