using System;
using System.ComponentModel.DataAnnotations;

namespace EducationalSystem.DAL.Models
{
    public class Discounts : BaseEntity
    {
        public int CourseID { get; set; }  // No need for [ForeignKey] here
        public int DiscountValue { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        // Navigation property
        public Courses Courses { get; set; } // Remains as a navigation property
    }
}
