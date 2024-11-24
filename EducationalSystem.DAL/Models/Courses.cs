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
        public Categories Categories { get; set; }
        public Course_Instructors Course_Instructors { get; set; }  // One Instructor per Course
        public ICollection<Lessons>? Lessons { get; set; }
        public ICollection<Discounts>? Discounts { get; set; }
        public ICollection<Assessments>? Assessments { get; set; }
        public ICollection<Course_Enrollments>? Course_Enrollments { get; set; }
    }
}
