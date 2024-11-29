using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EducationalSystem.DAL.Models.Config
{
    public class AssessmentConfiguration : IEntityTypeConfiguration<Assessments>
    {
        public void Configure(EntityTypeBuilder<Assessments> builder)
        {
            builder.HasKey(x => x.ID);

            builder.Property(x => x.AssessmentType).HasMaxLength(100);
            builder.Property(x => x.CreatedDate)
                .HasColumnType("datetime2")
                .HasPrecision(0)
                .IsRequired()
                .HasDefaultValueSql("GETDATE()");

            // Configure the foreign key for Courses
            builder
                .HasOne(a => a.Courses)          // Navigation property
                .WithMany(c => c.Assessments)   // Inverse navigation property
                .HasForeignKey(a => a.CourseID) // Foreign key property
                .OnDelete(DeleteBehavior.NoAction);  // Prevent cascading delete/update for CourseID

            // Configure the foreign key for Lessons
            builder
                .HasOne(a => a.Lessons)          // Navigation property
                .WithMany(l => l.Assessments)    // Inverse navigation property
                .HasForeignKey(a => a.LessonID)  // Foreign key property
                .OnDelete(DeleteBehavior.NoAction);  // Prevent cascading delete/update for LessonID
            
            builder
                .HasMany(t => t.TextSubmissions)
                .WithOne(a => a.Assessments)
                .HasForeignKey(a => a.AssessmentID)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasMany(t => t.FileSubmissions)
                .WithOne(a => a.Assessments)
                .HasForeignKey(a => a.AssessmentID)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasOne(t => t.Rubrics)
                .WithOne(a => a.Assessments)
                .HasForeignKey<Rubrics>(a => a.AssessmentID)
                .OnDelete(DeleteBehavior.NoAction);


        }
    }
}
