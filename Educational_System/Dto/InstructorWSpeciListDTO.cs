using EducationalSystem.DAL.Models;

namespace Educational_System.Dto
{
    public class InstructorWSpeciListDTO
    {
        public string Insname { get; set; } //  userTable
        public string? ProfileImageURL { get; set; }// users table
        public string InsphoneNumber { get; set; } // instructors
        public string InsNationalCardImg { get; set; } // instructors table
        public string CV_PDF_URL { get; set; } // instructors table
        public string BIO { get; set; } // instructors table
        public IQueryable<Specializations> specializations; // Specializations table
    }
}
