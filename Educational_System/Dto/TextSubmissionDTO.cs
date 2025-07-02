namespace Educational_System.Dto
{
    public class TextSubmissionDTO
    {
        public int AssessmentID { get; set; } // foreign key with Assessments table
        public string UserID { get; set; } // foreign key with ApplicationUser table
        public string ResponseText { get; set; }
        public int Grade { get; set; }
        public string Feedback { get; set; }
    }
}
