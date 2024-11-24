using System;
using System.Collections.Generic;

namespace EducationalSystem.DAL.Models
{
    public class Assessments : BaseEntity
    {
        public int CourseID { get; set; }
        public int? LessonID { get; set; }
        public string AssessmentType { get; set; }
        public int MaxScore { get; set; }
        public DateTime CreatedDate { get; set; }
        public ICollection<Questions> Questions { get; set; }
        public Courses Courses { get; set; }
        public Lessons? Lessons { get; set; }
    }
}
