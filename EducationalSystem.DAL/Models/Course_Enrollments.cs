namespace EducationalSystem.DAL.Models
{
    public class Course_Enrollments : BaseEntity
    {
        public string UserID { get; set; } // Foreign key for ApplicationUser
        public DateTime EnrollmentDate { get; set; }
        public int CourseId { get; set; } // Foreign key for Courses
        virtual public Courses Courses { get; set; }  // <-- Add this navigation property
        virtual public ApplicationUser User { get; set; } // Navigation property for ApplicationUser
    }
}
