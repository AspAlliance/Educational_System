namespace EducationalSystem.DAL.Models
{
    public class User_Instructor : BaseEntity
    {
        public string UserId { get; set; } // ForeignKey with ApplicationUser
        public int InstructorId { get; set; } // ForeignKey with Instructors

        virtual public Instructors Instructors { get; set; }
        virtual public ApplicationUser User { get; set; }
    }
}
