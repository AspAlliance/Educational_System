using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EducationalSystem.DAL.Models.Config
{
    public class ProgressConfiguration : IEntityTypeConfiguration<Progress>
    {
        public void Configure(EntityTypeBuilder<Progress> builder)
        {
            builder.Property(x => x.CompletedDate)
                .HasColumnType("datetime2")
                .HasPrecision(0)
                .IsRequired()
                .HasDefaultValueSql("GETDATE()");

            // Configuring the relationship with ApplicationUser
            builder
                .HasOne(p => p.User)
                .WithMany()  // No collection property in ApplicationUser for Progress
                .HasForeignKey(p => p.UserID)
                .OnDelete(DeleteBehavior.Cascade);  // Adjust delete behavior as needed

            // Configuring the relationship with Courses
            builder
                .HasOne(p => p.Courses)
                .WithMany()  // No collection property in Courses for Progress
                .HasForeignKey(p => p.CourseID)
                .OnDelete(DeleteBehavior.Cascade);  // Adjust delete behavior as needed

            // Key configuration
            builder.HasKey(x => x.ID);
        }
    }
}
