namespace EducationalSystem.DAL.Models
{
    public class Comments : BaseEntity
    {
        public int LessonID { get; set; } // Foreign key for Lessons
        public string UserID { get; set; } // Foreign key for ApplicationUser
        public string CommentText { get; set; }
        public DateTime PostedDate { get; set; }
        virtual public Lessons Lessons { get; set; } // Navigation property for Lessons
        virtual public ApplicationUser ApplicationUser { get; set; } // Navigation property for ApplicationUser
    }
}
