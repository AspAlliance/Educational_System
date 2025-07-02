namespace Educational_System.Dto
{
    public class AddQuestionDTO
    {
        public string QuestionTypeName { get; set; } // "MCQ" or "Essay"
        public string QuestionText { get; set; }
        public int QuestionTypeID { get; set; } 
        public int Points { get; set; }
        public List<string> Choices { get; set; } // Only for MCQ questions
        public string ChoiceText { get; set; }

    }
}
