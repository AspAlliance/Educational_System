using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EducationalSystem.DAL.Models.Config
{
    public class LessonPrerequisitesConfiguration : IEntityTypeConfiguration<Lesson_Prerequisites>
    {
        public void Configure(EntityTypeBuilder<Lesson_Prerequisites> builder)
        {
            // Primary Key
            builder.HasKey(x => x.ID);

            // Foreign Key: CurrentLessonID
            builder.HasOne(lp => lp.CurrentLesson)
                   .WithMany(l => l.CurrentLessonPrerequisites)
                   .HasForeignKey(lp => lp.CurrentLessonID)
                   .OnDelete(DeleteBehavior.NoAction); // Prevent cascading delete

            // Foreign Key: PrerequisiteLessonID
            builder.HasOne(lp => lp.PrerequisiteLesson)
                   .WithMany(l => l.PrerequisiteLessonPrerequisites)
                   .HasForeignKey(lp => lp.PrerequisiteLessonID)
                   .OnDelete(DeleteBehavior.NoAction); // Prevent cascading delete
        }
    }
}
