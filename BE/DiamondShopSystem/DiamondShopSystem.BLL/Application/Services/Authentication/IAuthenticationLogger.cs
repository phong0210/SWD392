namespace DiamondShopSystem.BLL.Application.Services.Authentication
{
    public interface IAuthenticationLogger
    {
        void LogLoginAttempt(string email, string ipAddress, bool success, string? failureReason = null);
        void LogPasswordResetRequest(string email, string ipAddress, bool success, string? failureReason = null);
        void LogPasswordReset(string email, string ipAddress, bool success, string? failureReason = null);
        void LogGoogleLoginAttempt(string email, string ipAddress, bool success, string? failureReason = null);
        void LogFailedLoginAttempt(string email, string ipAddress, string reason);
        void LogSuspiciousActivity(string email, string ipAddress, string activity, string details);
    }
} 