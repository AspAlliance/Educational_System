using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EducationalSystem.DAL.Models.Config
{
    public class CoursesConfiguration : IEntityTypeConfiguration<Courses>
    {
        public void Configure(EntityTypeBuilder<Courses> builder)
        {
            // Define properties
            builder.Property(x => x.CourseTitle)
                .HasMaxLength(100) // Set a maximum length for CourseTitle
                .IsRequired(); // CourseTitle is required

            builder.Property(x => x.Description)
                .HasMaxLength(500); // Set a maximum length for Description (optional)

            builder.Property(x => x.ThumbnailURL)
                .HasMaxLength(255); // Set a maximum length for Thumbnail URL

            builder.Property(x => x.CreatedDate)
                .HasColumnType("datetime2")
                .HasPrecision(0)
                .IsRequired()
                .HasDefaultValueSql("GETDATE()"); // Default value for CreatedDate is the current date

            builder.Property(x => x.TotalAmount)
                .HasPrecision(10, 2) // Set precision for TotalAmount (10 digits, 2 decimal places)
                .IsRequired(); // TotalAmount is required

            builder.HasKey(x => x.ID); // Set the primary key

            // Configure the relationship between Courses and Categories (many-to-one)
            builder.HasOne(c => c.Categories) // Each Course belongs to one Category
                .WithMany(cat => cat.Courses) // Each Category has many Courses
                .HasForeignKey(c => c.CategoryID) // Foreign key in Courses
                .OnDelete(DeleteBehavior.NoAction); // Cascade delete behavior

            // Configure the relationship between Courses and Course_Instructors (one-to-one)
            builder.HasOne(c => c.Course_Instructors) // Each Course has one Course_Instructor
                .WithOne(ci => ci.Courses) // Each Course_Instructor is associated with one Course
                .HasForeignKey<Course_Instructors>(ci => ci.CourseID) // Foreign key in Course_Instructors
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete behavior

            // Configure the relationship between Courses and Lessons (one-to-many)
            builder.HasMany(c => c.Lessons) // A Course can have many Lessons
                .WithOne(l => l.Courses) // Each Lesson belongs to one Course
                .HasForeignKey(l => l.CourseID) // Foreign key in Lessons
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete behavior

            // Configure the relationship between Courses and Discounts (one-to-many)
            builder.HasMany(c => c.Discounts) // A Course can have many Discounts
                .WithOne(d => d.Courses) // Each Discount belongs to one Course
                .HasForeignKey(d => d.CourseID) // Foreign key in Discounts
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete behavior

            // Configure the relationship between Courses and Assessments (one-to-many)
            builder.HasMany(c => c.Assessments) // A Course can have many Assessments
                .WithOne(a => a.Courses) // Each Assessment belongs to one Course
                .HasForeignKey(a => a.CourseID) // Foreign key in Assessments
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete behavior

            // Configure the relationship between Courses and Course_Enrollments (one-to-many)
            builder.HasMany(c => c.Course_Enrollments) // A Course can have many Course_Enrollments
                .WithOne(ce => ce.Courses) // Each Course_Enrollment belongs to one Course
                .HasForeignKey(ce => ce.CourseId) // Foreign key in Course_Enrollments
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete behavior
        }
    }
}
