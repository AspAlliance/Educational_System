using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace EducationalSystem.DAL.Models.Config
{
    public class TextSubmissionsConfiguration : IEntityTypeConfiguration<TextSubmissions>
    {
        public void Configure(EntityTypeBuilder<TextSubmissions> builder)
        {
            // Primary Key
            builder.HasKey(x => x.ID);

            // Configuring foreign key relationship with Assessments
            builder
                .HasOne(ts => ts.Assessments)
                .WithMany(a => a.TextSubmissions)  // Assuming Assessments has a collection of TextSubmissions
                .HasForeignKey(ts => ts.AssessmentID)
                .OnDelete(DeleteBehavior.NoAction); // Adjust delete behavior as needed

            // Configuring foreign key relationship with ApplicationUser
            builder
                .HasOne(ts => ts.ApplicationUser)
                .WithMany(u => u.TextSubmissions)  // Assuming ApplicationUser has a collection of TextSubmissions
                .HasForeignKey(ts => ts.UserID)
                .OnDelete(DeleteBehavior.NoAction); // Adjust delete behavior as needed
        }
    }
}
