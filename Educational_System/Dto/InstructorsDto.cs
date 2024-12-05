using EducationalSystem.DAL.Models;

namespace Educational_System.Dto
{
    public class InstructorsDto
    {
        public string InstructorName { get; set; }
        public string SpecializationsName { get; set; }
        public string BIO { get; set; }
        //Please don't implement a model list in DTO because it creates many conflicts. Change it as soon as possible.
        public List<Courses> Courses { get; set; } = new List<Courses>(); 
        //public List<string> CourseTitles { get; set; } = new List<string>();
    }
}
