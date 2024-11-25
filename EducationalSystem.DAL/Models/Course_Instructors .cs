namespace EducationalSystem.DAL.Models
{
    public class Course_Instructors : BaseEntity
    {
        public int CourseID { get; set; } // Foreign key for Courses
        public int InstructorID { get; set; } // Foreign key for Instructors

        // Navigation properties
        virtual public Courses Courses { get; set; } // Navigation property for Courses
        virtual public Instructors Instructors { get; set; } // Navigation property for Instructors
    }
}
