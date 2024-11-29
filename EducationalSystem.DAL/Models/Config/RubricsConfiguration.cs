using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationalSystem.DAL.Models.Config
{
    public class RubricsConfiguration : IEntityTypeConfiguration<Rubrics>
    {
        public void Configure(EntityTypeBuilder<Rubrics> builder)
        {
            // Primary Key
            builder.HasKey(x => x.ID);

            builder
              .HasOne(r => r.Assessments) // Rubrics has one Assessment
              .WithOne(a => a.Rubrics)    // Assessments has one Rubric
              .HasForeignKey<Rubrics>(r => r.AssessmentID) // Foreign key in Rubrics referencing Assessments
              .OnDelete(DeleteBehavior.NoAction); // You can adjust the delete behavior based on your needs

        }
    }
}
