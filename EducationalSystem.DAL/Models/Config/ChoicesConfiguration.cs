using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EducationalSystem.DAL.Models.Config
{
    public class ChoicesConfiguration : IEntityTypeConfiguration<Choices>
    {
        public void Configure(EntityTypeBuilder<Choices> builder)
        {
            builder.HasKey(x => x.ID);

            builder.Property(x => x.IsCorrect).IsRequired();
            builder.Property(x => x.ChoiceText).IsRequired();

            // Configuring the relationship between Choices and Questions
            builder
                .HasOne(c => c.Questions)      // Navigation property
                .WithMany(q => q.Choices)      // Inverse navigation in Questions
                .HasForeignKey(c => c.QuestionID) // Foreign key in Choices
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete behavior
        }
    }
}
