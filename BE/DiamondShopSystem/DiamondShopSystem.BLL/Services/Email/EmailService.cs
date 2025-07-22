using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Threading.Tasks;

namespace DiamondShopSystem.BLL.Services.Email
{
    public class EmailService : IEmailService
    {
        private readonly ILogger<EmailService> _logger;
        private readonly EmailSettings _emailSettings;

        public EmailService(ILogger<EmailService> logger, IOptions<EmailSettings> emailSettings)
        {
            _logger = logger;
            _emailSettings = emailSettings.Value;
        }

        public async Task SendOtpEmailAsync(string email, string otp)
        {
            try
            {
                using (var client = new SmtpClient(_emailSettings.SmtpHost))
                {
                    client.Port = _emailSettings.SmtpPort;
                    client.Credentials = new System.Net.NetworkCredential(_emailSettings.SmtpUser, _emailSettings.SmtpPass);
                    client.EnableSsl = true; // Most SMTP servers require SSL/TLS

                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress(_emailSettings.FromEmail),
                        Subject = "Your OTP for Diamond Shop System",
                        Body = $"Your One-Time Password (OTP) is: <b>{otp}</b>. This OTP is valid for 10 minutes.",
                        IsBodyHtml = true,
                    };
                    mailMessage.To.Add(email);

                    await client.SendMailAsync(mailMessage);
                    _logger.LogInformation("OTP email sent successfully to {Email}", email);
                }
            }
            catch (SmtpException ex)
            {
                _logger.LogError(ex, "Failed to send OTP email to {Email}. SMTP Error: {SmtpErrorCode}", email, ex.StatusCode);
                // Depending on your error handling strategy, you might rethrow or handle gracefully
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while sending OTP email to {Email}", email);
            }
        }
    }
}
