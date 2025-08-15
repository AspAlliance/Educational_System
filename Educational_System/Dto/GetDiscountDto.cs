namespace Educational_System.Dto
{
    public class GetDiscountDto
    {
        public int ID { get; set; }
        public int CourseID { get; set; }
        public int DiscountValue { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string CourseTitle { get; set; }
    }
}
