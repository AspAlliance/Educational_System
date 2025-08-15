using EducationalSystem.DAL.Models;

namespace Educational_System.Dto.Course
{
    public class postCourseDto
    {
        public string? CourseTitle { get; set; }
        public string? Description { get; set; }
        public int? CategoryID { get; set; }
        public int? duration { get; set; } // Duration in hours
        public DateTime? CreatedDate { get; set; }
        public string? ThumbnailURL { get; set; }
        public decimal? TotalAmount { get; set; }
        public string? level { get; set; } // Level of the course (e.g., Beginner, Intermediate, Advanced)

    }
}
