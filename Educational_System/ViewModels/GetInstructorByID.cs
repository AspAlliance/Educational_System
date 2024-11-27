using EducationalSystem.DAL.Models;

namespace Educational_System.ViewModels
{
    public class GetInstructorByID
    {
        public string InstructorName { get; set; }
        public string SpecializationsName { get; set; }
        public string BIO { get; set; }
        public string ProfileImageURL { get; set; }
        public string PhoneNumber { get; set; }
        public List<Courses>? Courses { get; set; } = new List<Courses>();


    }
}
