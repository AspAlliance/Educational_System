namespace EducationalSystem.DAL.Models
{
    public class Assessment_Results : BaseEntity
    {
        public string UserID { get; set; }
        public int Score { get; set; }
        public DateTime? AttemptDate { get; set; }
        virtual public ApplicationUser User { get; set; }
    }
}
