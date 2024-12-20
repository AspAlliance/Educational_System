using EducationalSystem.DAL.Models;

namespace Educational_System.Dto.Category
{
    public class CourseCategory
    {
        public int ID { get; set; }
        public string CourseTitle { get; set; }
    }
    public class getCategoryDto
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public int CourseCount { get; set; }
        public List<CourseCategory> courses { get; set; }
    }
}
