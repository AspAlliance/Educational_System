using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EducationalSystem.DAL.Models.Config
{
    public class QuestionsConfiguration : IEntityTypeConfiguration<Questions>
    {
        public void Configure(EntityTypeBuilder<Questions> builder)
        {
            builder.HasKey(x => x.ID);

            // Configuring the relationship with Assessments
            builder
                .HasOne(q => q.Assessments)
                .WithMany(a => a.Questions)  // Assuming Assessments has a collection property for Questions
                .HasForeignKey(q => q.AssessmentID)
                .OnDelete(DeleteBehavior.Cascade);  // Adjust delete behavior as needed

            // Configuring the relationship with QuestionType
            builder
                .HasOne(q => q.QuestionType)
                .WithMany()  // Assuming no collection property in QuestionType for Questions
                .HasForeignKey(q => q.QuestionTypeID)
                .OnDelete(DeleteBehavior.Cascade);  // Adjust delete behavior as needed

            // Configuring the relationship with Choices (one-to-many)
            builder
                .HasMany(q => q.Choices)
                .WithOne(c => c.Questions)  // Assuming Choices has a navigation property to Questions
                .HasForeignKey(c => c.QuestionID)
                .OnDelete(DeleteBehavior.Cascade);  // Adjust delete behavior as needed
        }
    }
}
