using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EducationalSystem.DAL.Models.Config
{
    public class UserInstructorConfiguration : IEntityTypeConfiguration<User_Instructor>
    {
        public void Configure(EntityTypeBuilder<User_Instructor> builder)
        {
            // Primary Key
            builder.HasKey(ui => ui.ID);

            // Configure the relationship with ApplicationUser
            builder.HasOne(ui => ui.User)  // Each User_Instructor has one ApplicationUser
                   .WithMany(x => x.User_Instructor)            // ApplicationUser can have many User_Instructors (no navigation property in ApplicationUser)
                   .HasForeignKey(ui => ui.UserId)  // Foreign Key to ApplicationUser
                   .OnDelete(DeleteBehavior.NoAction); // Optional: Set delete behavior as needed

            // Configure the relationship with Instructors
            builder.HasOne(ui => ui.Instructors)  // Each User_Instructor has one Instructor
                   .WithMany(x=>x.User_Instructors)                  // Instructors can have many User_Instructors (no navigation property in Instructors)
                   .HasForeignKey(ui => ui.InstructorId)  // Foreign Key to Instructors
                   .OnDelete(DeleteBehavior.NoAction); // Optional: Set delete behavior as needed
        }
    }
}
