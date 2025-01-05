namespace Educational_System.Dto.Lesson
{
    public class getLessonDto
    {
        public int ID { get; set; }
        public string LessonTitle { get; set; }
        public string Content { get; set; }
        public int LessonOrder { get; set; }
        public string LessonDescription { get; set; }
        public int SubLessonID { get; set; }
        public List<getLessonPrerequisiteDto> Prerequisites { get; set; }
    }
}