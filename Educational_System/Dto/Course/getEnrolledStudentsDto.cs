namespace Educational_System.Dto.Course
{
    public class StudentsCourse 
    {
        public string StudentName { get; set; }
        public string StudentEmail { get; set; }
    }

    public class getEnrolledStudentsDto
    {
        public int ID { get; set; }
        public string CourseTitle { get; set; }
        public string Description { get; set; }
        public string InstructorName { get; set; }
        public string CategoryName { get; set; }
        public int StudentCount { get; set; }
        public List<StudentsCourse> Students{ get; set; }
    }
}
