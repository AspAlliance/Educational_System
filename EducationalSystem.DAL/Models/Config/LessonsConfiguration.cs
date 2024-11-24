using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EducationalSystem.DAL.Models.Config
{
    public class LessonsConfiguration : IEntityTypeConfiguration<Lessons>
    {
        public void Configure(EntityTypeBuilder<Lessons> builder)
        {
            builder.Property(x => x.LessonTitle).HasMaxLength(50).IsRequired();

            builder.Property(x => x.CreatedDate)
                .HasColumnType("datetime2")
                .HasPrecision(0)
                .IsRequired()
                .HasDefaultValueSql("GETDATE()");

            // Configuring foreign key for Courses with NO ACTION to avoid cascade deletion
            builder
                .HasOne(l => l.Courses)
                .WithMany(c => c.Lessons)
                .HasForeignKey(l => l.CourseID)
                .OnDelete(DeleteBehavior.NoAction); // Prevent cascading delete or update on Lessons -> Courses

            // Configuring the relationship with Assessments
            builder
                .HasMany(l => l.Assessments)
                .WithOne(a => a.Lessons)
                .HasForeignKey(a => a.LessonID)
                .OnDelete(DeleteBehavior.SetNull); // Prevent cascading delete or update on Assessments -> Lessons

            // Key configuration
            builder.HasKey(x => x.ID);
        }
    }
}
