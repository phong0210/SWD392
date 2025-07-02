using Microsoft.Extensions.Logging;

namespace DiamondShopSystem.BLL.Application.Services.Authentication
{
    public class AuthenticationLogger : IAuthenticationLogger
    {
        private readonly ILogger<AuthenticationLogger> _logger;

        public AuthenticationLogger(ILogger<AuthenticationLogger> logger)
        {
            _logger = logger;
        }

        public void LogLoginAttempt(string email, string ipAddress, bool success, string? failureReason = null)
        {
            if (success)
            {
                _logger.LogInformation("Successful login attempt for user {Email} from IP {IpAddress}", 
                    email, ipAddress);
            }
            else
            {
                _logger.LogWarning("Failed login attempt for user {Email} from IP {IpAddress}. Reason: {Reason}", 
                    email, ipAddress, failureReason ?? "Invalid credentials");
            }
        }

        public void LogPasswordResetRequest(string email, string ipAddress, bool success, string? failureReason = null)
        {
            if (success)
            {
                _logger.LogInformation("Password reset requested for user {Email} from IP {IpAddress}", 
                    email, ipAddress);
            }
            else
            {
                _logger.LogWarning("Failed password reset request for user {Email} from IP {IpAddress}. Reason: {Reason}", 
                    email, ipAddress, failureReason ?? "Unknown error");
            }
        }

        public void LogPasswordReset(string email, string ipAddress, bool success, string? failureReason = null)
        {
            if (success)
            {
                _logger.LogInformation("Password successfully reset for user {Email} from IP {IpAddress}", 
                    email, ipAddress);
            }
            else
            {
                _logger.LogWarning("Failed password reset for user {Email} from IP {IpAddress}. Reason: {Reason}", 
                    email, ipAddress, failureReason ?? "Unknown error");
            }
        }

        public void LogGoogleLoginAttempt(string email, string ipAddress, bool success, string? failureReason = null)
        {
            if (success)
            {
                _logger.LogInformation("Successful Google login for user {Email} from IP {IpAddress}", 
                    email, ipAddress);
            }
            else
            {
                _logger.LogWarning("Failed Google login for user {Email} from IP {IpAddress}. Reason: {Reason}", 
                    email, ipAddress, failureReason ?? "Invalid token");
            }
        }

        public void LogFailedLoginAttempt(string email, string ipAddress, string reason)
        {
            _logger.LogWarning("Failed login attempt for user {Email} from IP {IpAddress}. Reason: {Reason}", 
                email, ipAddress, reason);
        }

        public void LogSuspiciousActivity(string email, string ipAddress, string activity, string details)
        {
            _logger.LogError("Suspicious activity detected for user {Email} from IP {IpAddress}. Activity: {Activity}. Details: {Details}", 
                email, ipAddress, activity, details);
        }
    }
} 