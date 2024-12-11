namespace Educational_System.Helpers
{
    using MailKit.Net.Smtp;
    using MimeKit;
    using MailKit.Security; // Add this namespace for SecureSocketOptions
    using System.Threading.Tasks;

    public class EmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendResetPasswordEmail(string toEmail, string resetLink)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(
                _configuration["EmailSettings:SenderName"],  // Sender's name
                _configuration["EmailSettings:SenderEmail"]  // Sender's email address
            ));
            email.To.Add(new MailboxAddress("", toEmail)); // Recipient's email address
            email.Subject = "Password Reset Request";

            email.Body = new TextPart("html")
            {
                Text = $@"
                    <p>Hi,</p>
                    <p>You requested to reset your password. Please click the link below to reset your password:</p>
                    <p><a href='{resetLink}'>Reset Password</a></p>
                    <p>If you didn't request this, please ignore this email.</p>
                    <p>Thanks,<br>Your App Team</p>
                "
            };

            using var smtp = new SmtpClient();
            try
            {
                // Connect using STARTTLS instead of SSL on port 587
                await smtp.ConnectAsync(_configuration["EmailSettings:SmtpServer"],
                                        int.Parse(_configuration["EmailSettings:Port"]),
                                        SecureSocketOptions.StartTls);  // Use STARTTLS here

                await smtp.AuthenticateAsync(
                    _configuration["EmailSettings:Username"],
                    _configuration["EmailSettings:Password"]
                );

                await smtp.SendAsync(email);
            }
            finally
            {
                await smtp.DisconnectAsync(true);
            }
        }
    }
}
