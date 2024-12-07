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

        // Use enum for status
        public InstructorStatus? Status { get; set; }

        // Navigation properties
        virtual public Specializations? Specializations { get; set; }
        virtual public ApplicationUser? applicationUser { get; set; }
        virtual public ICollection<User_Instructor>? User_Instructors { get; set; }
        virtual public ICollection<Course_Instructors>? Course_Instructors { get; set; }
    }

    // Enum inside the same class
    public enum InstructorStatus
    {
        Pending = 0,
        Approved = 1,
        Rejected = 2
    }
}
