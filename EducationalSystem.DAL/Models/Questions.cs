namespace EducationalSystem.DAL.Models
{
    public class Questions : BaseEntity
    {
        public int AssessmentID { get; set; } // Foreign key for Assessments
        public string QuestionText { get; set; }
        public int QuestionTypeID { get; set; } // Foreign key for QuestionType
        public int Points { get; set; }

        // Navigation properties
        virtual public ICollection<Choices> Choices { get; set; }
        virtual public QuestionType QuestionType { get; set; }
        virtual public Assessments Assessments { get; set; }
    }
}
