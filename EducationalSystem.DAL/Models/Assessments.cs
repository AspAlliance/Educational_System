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
        virtual public ICollection<Questions>? Questions { get; set; }
        virtual public Courses Courses { get; set; }
        virtual public Lessons? Lessons { get; set; }
        virtual public ICollection<TextSubmissions>? TextSubmissions { get; set; }
        virtual public ICollection<FileSubmissions>? FileSubmissions { get; set; }
        virtual public Rubrics? Rubrics { get; set; }
    }
}
