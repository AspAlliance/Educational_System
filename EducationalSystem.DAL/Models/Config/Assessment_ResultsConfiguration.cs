using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EducationalSystem.DAL.Models.Config
{
    public class Assessment_ResultsConfiguration : IEntityTypeConfiguration<Assessment_Results>
    {
        public void Configure(EntityTypeBuilder<Assessment_Results> builder)
        {
            builder.HasKey(x => x.ID);

            builder.Property(x => x.AttemptDate)
                .HasColumnType("datetime2")
                .HasPrecision(0)
                .IsRequired()
                .HasDefaultValueSql("GETDATE()");

            // Configuring the relationship using Fluent API
            builder.HasOne(x => x.User)  // Navigation property
                .WithMany()              // Assuming no inverse navigation property on ApplicationUser
                .HasForeignKey(x => x.UserID)  // Foreign key property
                .OnDelete(DeleteBehavior.Cascade);  // Delete behavior for cascading

            // Optionally, set the column name for the foreign key
            builder.Property(x => x.UserID).HasColumnName("UserID");
        }
    }
}
