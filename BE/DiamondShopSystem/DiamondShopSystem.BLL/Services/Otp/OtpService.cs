using System;
using System.Security.Cryptography;

namespace DiamondShopSystem.BLL.Services.Otp
{
    public class OtpService : IOtpService
    {
        private const int OtpLength = 6;

        public string GenerateOtp()
        {
            var randomBytes = new byte[OtpLength];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
            }

            var otpChars = new char[OtpLength];
            for (int i = 0; i < OtpLength; i++)
            {
                otpChars[i] = (char)('0' + (randomBytes[i] % 10));
            }

            return new string(otpChars);
        }
    }
}
