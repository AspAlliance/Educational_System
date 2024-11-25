using Microsoft.AspNetCore.Identity;

namespace EducationalSystem.DAL.Models
{
    public class ApplicationUser:IdentityUser
    {
        public string Name { get; set; }
        public string ProfileImageURL { get; set; }
        virtual public ICollection<User_Instructor>? User_Instructor { get; set; }
        virtual public ICollection<Comments> Comments { get; set; }
        virtual public ICollection<Lesson_Completions> Lesson_Completions { get; set; }
        virtual public ICollection<Assessment_Results> Assessment_Results { get; set; }
        virtual public ICollection<Progress> progresses { get; set; }
        virtual public Instructors Instructors { get; set; }
        virtual public ICollection<Course_Enrollments> CourseEnrollments { get; set; } // Navigation property

        /*o One-to-Many with Instructors
o One-to-Many with Comments
o One-to-Many with Lesson_Completions
o One-to-Many with Assessment_Results
o One-to-Many with Progress
*/
    }
}
