namespace Educational_System.Dto
{
    public class ChangePasswordDto
    {
        public string UserName { get; set; } // The username of the user
        public string CurrentPassword { get; set; } // The current password
        public string NewPassword { get; set; } // The new password
    }
}