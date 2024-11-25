namespace EducationalSystem.DAL.Models
{
    public class Lesson_Completions : BaseEntity
    {
        public string UserID { get; set; }  // Foreign key for ApplicationUser
        public int LessonID { get; set; }  // Foreign key for Lessons
        public DateTime CompletionDate { get; set; }  // Completion date of the lesson

        // Navigation properties
        virtual public Lessons Lessons { get; set; }  // Navigation property for Lessons
        virtual public ApplicationUser ApplicationUser { get; set; }  // Navigation property for ApplicationUser
    }
}
