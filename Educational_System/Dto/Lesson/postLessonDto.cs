namespace Educational_System.Dto.Lesson
{
    public class postLessonDto
    {
        public string LessonTitle { get; set; }
        public string Content { get; set; }
        public int LessonOrder { get; set; }
        public string LessonDescription { get; set; }
        public int SubLessonID { get; set; }
        public IFormFile Video { get; set; } // upload video in the server
        public int? Duration { get; set; } // Duration of the lesson, e.g., "1 hour 30 minutes"
    }
}