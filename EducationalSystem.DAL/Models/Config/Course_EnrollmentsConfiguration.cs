using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EducationalSystem.DAL.Models.Config
{
    public class Course_EnrollmentsConfiguration : IEntityTypeConfiguration<Course_Enrollments>
    {
        public void Configure(EntityTypeBuilder<Course_Enrollments> builder)
        {
            builder.HasKey(x => x.ID);

            builder.Property(x => x.EnrollmentDate)
                .HasColumnType("datetime2")
                .HasPrecision(0)
                .IsRequired()
                .HasDefaultValueSql("GETDATE()");

            // Configuring the relationship between Course_Enrollments and Courses
            builder
                .HasOne(ce => ce.Courses)         // Navigation property in Course_Enrollments
                .WithMany(c => c.Course_Enrollments) // Inverse navigation in Courses
                .HasForeignKey(ce => ce.CourseId) // Foreign key in Course_Enrollments
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete behavior

            // Configuring the relationship between Course_Enrollments and ApplicationUser
            builder
                .HasOne(ce => ce.User)          // Navigation property in Course_Enrollments
                .WithMany(u => u.CourseEnrollments) // Inverse navigation in ApplicationUser
                .HasForeignKey(ce => ce.UserID) // Foreign key in Course_Enrollments
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete behavior
        }
    }
}
