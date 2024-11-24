namespace EducationalSystem.DAL.Models
{
    public class Progress : BaseEntity
    {
        public string UserID { get; set; } // Foreign key for ApplicationUser
        public int CourseID { get; set; } // Foreign key for Courses
        public int Score { get; set; }
        public DateTime CompletedDate { get; set; }

        // Navigation properties
        public ApplicationUser User { get; set; }
        public Courses Courses { get; set; }
    }
}
