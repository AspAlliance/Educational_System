using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EducationalSystem.DAL.Models.Config
{
    public class DiscountsConfiguration : IEntityTypeConfiguration<Discounts>
    {
        public void Configure(EntityTypeBuilder<Discounts> builder)
        {
            // Define the primary key
            builder.HasKey(d => d.ID);

            // Configure the foreign key for Courses
            builder
                .HasOne(d => d.Courses) // Navigation property
                .WithMany(c => c.Discounts) // The Courses class should have a collection of Discounts
                .HasForeignKey(d => d.CourseID) // Define the foreign key
                .OnDelete(DeleteBehavior.Cascade);  // You can change the delete behavior (Cascade, NoAction, etc.)

            // Configure other properties
            builder.Property(d => d.DiscountValue).IsRequired();
            builder.Property(d => d.StartDate).IsRequired();
            builder.Property(d => d.EndDate).IsRequired();
        }
    }
}
