using EducationalSystem.DAL.Models;

namespace Educational_System.Dto
{
    public class LessonCourseCommentDTO
    {
        public string lessonTitle { get; set; }
        public string crsName { get; set; }
        public string contect { get; set; }
        public DateTime? createdDate { get; set; }
        public string lessonDescription { get; set; }
        public List<Comments> comments { get; set; }
    }
}
