using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EducationalSystem.DAL.Models.Config
{
    public class QuestionTypeConfiguration : IEntityTypeConfiguration<QuestionType>
    {
        public void Configure(EntityTypeBuilder<QuestionType> builder)
        {
            builder.HasKey(qt => qt.ID);  // Primary key

            builder.Property(qt => qt.QuestionTypeName)
                .HasMaxLength(100)  // Limit the length of QuestionTypeName
                .IsRequired();      // Make it a required field

            // Optionally, if you want to configure the table name, you can do so here
            builder.ToTable("QuestionTypes");
        }
    }
}
