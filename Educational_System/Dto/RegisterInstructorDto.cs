using Microsoft.AspNetCore.Http;

namespace Educational_System.Dto
{
    public class RegisterInstructorDto
    {
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Bio { get; set; }
        public int SpecializationID { get; set; }

        public IFormFile CV_PDF { get; set; }
        public IFormFile NationalCardImage { get; set; }
    }
}
