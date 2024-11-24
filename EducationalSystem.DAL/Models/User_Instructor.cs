namespace EducationalSystem.DAL.Models
{
    public class User_Instructor : BaseEntity
    {
        public string UserId { get; set; } // ForeignKey with ApplicationUser
        public int InstructorId { get; set; } // ForeignKey with Instructors
        //test
        public Instructors Instructors { get; set; }
        public ApplicationUser User { get; set; }
    }
}
