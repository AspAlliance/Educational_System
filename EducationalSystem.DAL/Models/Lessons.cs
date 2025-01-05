namespace EducationalSystem.DAL.Models
{
    public class Lessons : BaseEntity
    {
        public int CourseID { get; set; }  // Foreign key for Courses
        public string LessonTitle { get; set; }
        public string Content { get; set; }
        public int LessonOrder { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.Now;
        public int SubLessonID { get; set; } // Foreign key for Courses
        public string LessonDescription { get; set; }

        // Inverse navigation for CurrentLesson
        virtual public ICollection<Lesson_Prerequisites> CurrentLessonPrerequisites { get; set; }

        // Inverse navigation for PrerequisiteLesson
        virtual public ICollection<Lesson_Prerequisites> PrerequisiteLessonPrerequisites { get; set; }
        virtual public ICollection<Assessments>? Assessments { get; set; }
        virtual public ICollection<Comments> Comments { get; set; }
        virtual public ICollection<Lesson_Completions> Lesson_Completions { get; set; }

        // Navigation property for Courses
        virtual public Courses Courses { get; set; }
        virtual public SubLessons SubLessons { get; set; }
    }
}
