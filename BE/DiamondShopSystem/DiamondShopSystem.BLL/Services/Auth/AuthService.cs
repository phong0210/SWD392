using System;
using System.Security.Cryptography;
using System.Text;
using DiamondShopSystem.DAL.Entities;

namespace DiamondShopSystem.BLL.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly JwtUtil _jwtUtil;

        public AuthService(JwtUtil jwtUtil)
        {
            _jwtUtil = jwtUtil;
        }

        public string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }

        public async Task<string> GenerateJwtTokenAsync(DiamondShopSystem.DAL.Entities.User user)
        {
            return await _jwtUtil.GenerateTokenAsync(user);
        }

        public bool ValidatePassword(string password, string hashedPassword)
        {
            var hashedInput = HashPassword(password);
            return hashedInput == hashedPassword;
        }
    }
} 