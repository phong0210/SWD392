using Microsoft.Extensions.Configuration;
using OtpNet;
using System.Security.Cryptography;
using System.Text;


namespace DiamondShopSystem.BLL.Utils
{
    public class OTPUtil : IOTPUtil
    {
        private readonly string _secretSalt;

        public OTPUtil(IConfiguration configuration)
        {
            _secretSalt = configuration["OtpSettings:SecretSalt"];
        }

        private string GenerateSecretKey(string email)
        {
            using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(_secretSalt)))
            {
                byte[] hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(email));
                return Base32Encoding.ToString(hash);
            }
        }

        public virtual string GenerateOtp(string email)
        {
            var secretKey = GenerateSecretKey(email);
            var totp = new Totp(Base32Encoding.ToBytes(secretKey));
            var otp = totp.ComputeTotp();
            return otp;
        }
    }
}
