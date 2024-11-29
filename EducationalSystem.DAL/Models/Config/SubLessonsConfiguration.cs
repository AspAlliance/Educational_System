using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace EducationalSystem.DAL.Models.Config
{
    public class SubLessonsConfiguration : IEntityTypeConfiguration<SubLessons>
    {
        public void Configure(EntityTypeBuilder<SubLessons> builder)
        {
            // Primary Key
            builder.HasKey(x => x.ID);

            // Configuring the foreign key relationship with Courses
            builder
                .HasOne(s => s.Courses)
                .WithMany(c => c.SubLessons)  // Assuming Courses has a collection of SubLessons
                .HasForeignKey(s => s.CourseID)
                .OnDelete(DeleteBehavior.NoAction); // Adjust the delete behavior as needed

            // Configuring the relationship with Lessons
            builder
                .HasMany(s => s.Lessons)
                .WithOne(l => l.SubLessons)
                .HasForeignKey(l => l.SubLessonID) // Linking to the SubLessonID in the Lessons table
                .OnDelete(DeleteBehavior.NoAction); // Adjust the delete behavior as needed
        }
    }
}
