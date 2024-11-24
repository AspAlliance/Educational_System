using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EducationalSystem.DAL.Models.Config
{
    public class Lesson_CompletionsConfiguration : IEntityTypeConfiguration<Lesson_Completions>
    {
        public void Configure(EntityTypeBuilder<Lesson_Completions> builder)
        {
            // Configure the primary key (composite key using UserID and LessonID)
            builder.HasKey(x => new { x.UserID, x.LessonID });

            // Configure the CompletionDate property
            builder.Property(x => x.CompletionDate)
                .HasColumnType("datetime2")
                .HasPrecision(0)
                .IsRequired()
                .HasDefaultValueSql("GETDATE()");  // Default value of current date and time

            // Configure the relationship between Lesson_Completions and Lessons (many-to-one)
            builder.HasOne(x => x.Lessons)  // Each Lesson_Completions belongs to one Lesson
                .WithMany(l => l.Lesson_Completions)  // A Lesson can have many Lesson_Completions
                .HasForeignKey(x => x.LessonID)  // Foreign key in Lesson_Completions
                .OnDelete(DeleteBehavior.Cascade);  // Cascade delete behavior

            // Configure the relationship between Lesson_Completions and ApplicationUser (many-to-one)
            builder.HasOne(x => x.ApplicationUser)  // Each Lesson_Completions belongs to one ApplicationUser
                .WithMany()  // ApplicationUser doesn't have a collection property for Lesson_Completions
                .HasForeignKey(x => x.UserID)  // Foreign key in Lesson_Completions
                .OnDelete(DeleteBehavior.Cascade);  // Cascade delete behavior

            // Ensure the ID is the primary key
            builder.HasKey(x => x.ID);
        }
    }
}
