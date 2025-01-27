namespace Educational_System.Dto
{
    public class QuestionAssessmentDTO
    {
        public int QuestionId { get; set; }
       //public string AssesmentName { get; set; } // from assesment table
        public string QuestionText { get; set; } // from question table
        public int Points { get; set; } // from question table
       // public string QuestionTypeName { get; set; }// from QuestionType Table
    }
}
