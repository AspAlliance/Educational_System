namespace EducationalSystem.DAL.Models
{
    public class Course_Enrollments : BaseEntity
    {
        public string UserID { get; set; } // Foreign key for ApplicationUser
        public DateTime EnrollmentDate { get; set; }
        public int CourseId { get; set; } // Foreign key for Courses
        public Courses Courses { get; set; }  // <-- Add this navigation property
        public ApplicationUser User { get; set; } // Navigation property for ApplicationUser
    }
}
