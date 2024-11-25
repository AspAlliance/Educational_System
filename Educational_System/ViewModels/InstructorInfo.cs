using EducationalSystem.DAL.Models;

namespace Educational_System.ViewModels
{
    public class InstructorInfo
    {
        public string InstructorName { get; set; }
        public string SpecializationsName { get; set; }
        public string BIO { get; set; }
        public List<Courses> Courses { get; set; } = new List<Courses>();
    }
}
