namespace EducationalSystem.DAL.Models
{
    public class Instructors : BaseEntity
    {
        public string PhoneNumber { get; set; }
        public string CV_PDF_URL { get; set; }
        public string NationalCardImageURL { get; set; }
        public string BIO { get; set; }
        public int? SpecializationsID { get; set; }
        public string UserID { get; set; }

        // Navigation properties
        public Specializations? Specializations { get; set; }
        public ApplicationUser? applicationUser { get; set; }
        public ICollection<User_Instructor>? User_Instructors { get; set; } // Many-to-many relationship via User_Instructor
        public ICollection<Course_Instructors>? Course_Instructors { get; set; }  // One-to-many relationship with Course_Instructors
    }
}
