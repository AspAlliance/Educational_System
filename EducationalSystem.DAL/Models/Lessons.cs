namespace EducationalSystem.DAL.Models
{
    public class Lessons : BaseEntity
    {
        public int CourseID { get; set; }  // Foreign key for Courses
        public string LessonTitle { get; set; }
        public string Content { get; set; }
        public int LessonOrder { get; set; }
        public DateTime? CreatedDate { get; set; }

        // Inverse navigation for CurrentLesson
        public ICollection<Lesson_Prerequisites> CurrentLessonPrerequisites { get; set; }

        // Inverse navigation for PrerequisiteLesson
        public ICollection<Lesson_Prerequisites> PrerequisiteLessonPrerequisites { get; set; }
        public ICollection<Assessments>? Assessments { get; set; }
        public ICollection<Comments> Comments { get; set; }
        public ICollection<Lesson_Completions> Lesson_Completions { get; set; }

        // Navigation property for Courses
        public Courses Courses { get; set; }
    }
}
