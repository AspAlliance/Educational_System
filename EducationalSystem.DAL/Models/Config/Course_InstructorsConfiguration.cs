using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EducationalSystem.DAL.Models.Config
{
    public class Course_InstructorsConfiguration : IEntityTypeConfiguration<Course_Instructors>
    {
        public void Configure(EntityTypeBuilder<Course_Instructors> builder)
        {
            // Define primary key for Course_Instructors
            builder.HasKey(x => x.ID);

            // Configure the relationship between Course_Instructors and Courses
            builder.HasOne(ci => ci.Courses)
                .WithOne(c => c.Course_Instructors) // A Course has only one Course_Instructor
                .HasForeignKey<Course_Instructors>(ci => ci.CourseID) // Foreign key in Course_Instructors
                .OnDelete(DeleteBehavior.NoAction); // Cascade delete behavior

            // Configure the relationship between Course_Instructors and Instructors
            builder.HasOne(ci => ci.Instructors)
                .WithMany(i => i.Course_Instructors) // An Instructor can have many Course_Instructors
                .HasForeignKey(ci => ci.InstructorID) // Foreign key in Course_Instructors
                .OnDelete(DeleteBehavior.NoAction); // Cascade delete behavior
        }
    }
}