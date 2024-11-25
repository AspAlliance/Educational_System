namespace EducationalSystem.DAL.Models
{
    public class Lesson_Prerequisites : BaseEntity
    {
        public int CurrentLessonID { get; set; }  // Foreign key for CurrentLesson
        public int PrerequisiteLessonID { get; set; }  // Foreign key for PrerequisiteLesson

        // Navigation properties
        virtual public Lessons CurrentLesson { get; set; }  // Navigation property for CurrentLesson
        virtual public Lessons PrerequisiteLesson { get; set; }  // Navigation property for PrerequisiteLesson
    }
}
