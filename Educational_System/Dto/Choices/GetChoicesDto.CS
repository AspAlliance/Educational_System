namespace Educational_System.Dto.Choices
{
    public class GetChoicesDto
    {
        public int ID { get; set; }
        public int QuestionID { get; set; }
        public string ChoiceText { get; set; }
        public int IsCorrect { get; set; }
    }
}