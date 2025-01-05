namespace Educational_System.Dto.Lesson
{
    public class postSubLessonDto
    {
        public int CourseID { get; set; } // foreign key with courses
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
