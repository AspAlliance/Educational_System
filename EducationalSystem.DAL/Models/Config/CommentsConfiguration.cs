using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EducationalSystem.DAL.Models.Config
{
    public class CommentsConfiguration : IEntityTypeConfiguration<Comments>
    {
        public void Configure(EntityTypeBuilder<Comments> builder)
        {
            builder.HasKey(x => x.ID);

            builder.Property(x => x.CommentText).IsRequired();

            builder.Property(x => x.PostedDate)
                .HasColumnType("datetime2")
                .HasPrecision(0)
                .IsRequired()
                .HasDefaultValueSql("GETDATE()");

            // Configuring the relationship between Comments and Lessons
            builder
                .HasOne(c => c.Lessons)        // Navigation property in Comments
                .WithMany(l => l.Comments)     // Inverse navigation in Lessons
                .HasForeignKey(c => c.LessonID) // Foreign key in Comments
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete behavior

            // Configuring the relationship between Comments and ApplicationUser
            builder
                .HasOne(c => c.ApplicationUser) // Navigation property in Comments
                .WithMany(u => u.Comments)      // Inverse navigation in ApplicationUser
                .HasForeignKey(c => c.UserID)   // Foreign key in Comments
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete behavior
        }
    }
}
