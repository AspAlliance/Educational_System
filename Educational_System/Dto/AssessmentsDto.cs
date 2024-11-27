using EducationalSystem.DAL.Models;

namespace Educational_System.Dto
{
    public class AssessmentsDto
    {
        public int CourseID { get; set; }
        public int? LessonID { get; set; }
        public string AssessmentType { get; set; }
        public int MaxScore { get; set; }
        public DateTime CreatedDate { get; set; } 

    }
}
