using System.ComponentModel.DataAnnotations;

namespace Educational_System.Dto
{
    public class AssessmentResultDto
    {
        //public int crsId {  get; set; }
        //public string crsName { get; set; }
        public string UserId { get; set; }
        [Range(0, 100, ErrorMessage = "Score must be between 0 and 100.")]
        public int Score { get; set; }
        public DateTime? AttemptDate { get; set; }
        public string? StudentName { get; set; }
    }
}
