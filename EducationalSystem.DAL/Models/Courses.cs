namespace EducationalSystem.DAL.Models
{
    public class Courses : BaseEntity
    {
        public string CourseTitle { get; set; }
        public string Description { get; set; }
        public int CategoryID { get; set; } // Foreign key for Categories
        public DateTime? CreatedDate { get; set; }
        public string ThumbnailURL { get; set; }
        public decimal TotalAmount { get; set; }

        // Navigation properties
        virtual public Categories Categories { get; set; }
        virtual public Course_Instructors Course_Instructors { get; set; }  // One Instructor per Course
        virtual public ICollection<Lessons>? Lessons { get; set; }
        virtual public ICollection<SubLessons> SubLessons { get; set; }
        virtual public ICollection<Discounts>? Discounts { get; set; }
        virtual public ICollection<Assessments>? Assessments { get; set; }
        virtual public ICollection<Course_Enrollments>? Course_Enrollments { get; set; }
    }
}
