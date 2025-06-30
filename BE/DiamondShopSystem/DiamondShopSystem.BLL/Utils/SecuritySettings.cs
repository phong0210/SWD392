namespace DiamondShopSystem.BLL.Utils
{
    public class SecuritySettings
    {
        public int MaxLoginAttemptsPerHour { get; set; } = 5;
        public int MaxPasswordResetRequestsPerHour { get; set; } = 3;
        public int PasswordResetTokenExpiryMinutes { get; set; } = 15;
        public int OTPExpiryMinutes { get; set; } = 10;
        public bool RequireEmailVerification { get; set; } = true;
        public bool LogSuspiciousActivity { get; set; } = true;
        public string[] AllowedOrigins { get; set; } = Array.Empty<string>();
        public bool EnableRateLimiting { get; set; } = true;
    }
} 