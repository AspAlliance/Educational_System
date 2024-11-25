namespace EducationalSystem.DAL.Models
{
    public class Choices : BaseEntity
    {
        public int QuestionID { get; set; } // Foreign key
        public string ChoiceText { get; set; }
        public int IsCorrect { get; set; }
        virtual public Questions Questions { get; set; } // Navigation property
    }
}
