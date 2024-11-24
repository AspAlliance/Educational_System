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
