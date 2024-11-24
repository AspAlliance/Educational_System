using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EducationalSystem.DAL.Models.Config
{
    public class CategoriesConfiguration : IEntityTypeConfiguration<Categories>
    {
        public void Configure(EntityTypeBuilder<Categories> builder)
        {
            builder.HasKey(x => x.ID);

            builder.Property(x => x.CategoryName)
                .HasMaxLength(50)
                .IsRequired();

            // Configuring the relationship with Courses
            builder
                .HasMany(c => c.Courses)       // Navigation property for related Courses
                .WithOne(c => c.Categories)   // Inverse navigation property in Courses
                .HasForeignKey(c => c.CategoryID) // Foreign key in the Courses table
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete behavior
        }
    }
}
