using System.ComponentModel.DataAnnotations;

namespace Educational_System.Dto
{
    public class AddAssesmentDTO
    {
        public string UserId { get; set; }
        [Range(0, 100, ErrorMessage = "Score must be between 0 and 100.")]
        public int Score { get; set; }
    }
}
