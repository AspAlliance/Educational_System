using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace EducationalSystem.DAL.Models.Context
{
    public class Education_System : IdentityDbContext<ApplicationUser>
    {
        public Education_System() : base()
        {
        }

        public Education_System(DbContextOptions<Education_System> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        // Specify your DbSet properties as is...
        public DbSet<Courses> Courses { get; set; }
        public DbSet<Course_Enrollments> Course_Enrollments { get; set; }
        public DbSet<Categories> Categories { get; set; }
        public DbSet<Instructors> Instructors { get; set; }
        public DbSet<Assessments> Assessments { get; set; }
        public DbSet<Assessment_Results> Assessment_Results { get; set; }
        public DbSet<Choices> Choices { get; set; }
        public DbSet<Lesson_Completions> Lesson_Completions { get; set; }
        public DbSet<Lessons> Lessons { get; set; }
        public DbSet<Progress> progresses { get; set; }
        public DbSet<Questions> Questions { get; set; }
        public DbSet<Comments> Comments { get; set; }
        public DbSet<Discounts> Discounts { get; set; }
        public DbSet<Lesson_Prerequisites> Lesson_Prerequisites { get; set; }
        public DbSet<QuestionType> questionTypes { get; set; }
        public DbSet<Specializations> Specializations { get; set; }
        public DbSet<User_Instructor> User_Instructor { get; set; }
        public DbSet<SubLessons> SubLessons { get; set; }
        public DbSet<Rubrics> Rubrics { get; set; }
        public DbSet<Course_Instructors> Course_Instructors { get; set; }
    }
}
