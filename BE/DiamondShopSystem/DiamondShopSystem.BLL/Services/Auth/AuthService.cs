using System;
using System.Security.Cryptography;
using System.Text;
using DiamondShopSystem.BLL.Services.User;
using DiamondShopSystem.DAL.Entities;

namespace DiamondShopSystem.BLL.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly JwtUtil _jwtUtil;
        private readonly IUserService _userService;
        private const int SaltSize = 16; // 128 bit
        private const int KeySize = 32; // 256 bit
        private const int Iterations = 10000;
        private static readonly HashAlgorithmName _hashAlgorithmName = HashAlgorithmName.SHA256;
        private const char Delimiter = ';';

        public AuthService(JwtUtil jwtUtil, IUserService userService)
        {
            _jwtUtil = jwtUtil;
            _userService = userService;
        }

        public string HashPassword(string password)
        {
            var salt = RandomNumberGenerator.GetBytes(SaltSize);
            var hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, _hashAlgorithmName, KeySize);

            return string.Join(Delimiter, Convert.ToBase64String(salt), Convert.ToBase64String(hash));
        }

        public bool ValidatePassword(string password, string hashedPassword)
        {
            var parts = hashedPassword.Split(Delimiter);
            if (parts.Length != 2)
            {
                // Or throw an exception for invalid format
                return false;
            }

            var salt = Convert.FromBase64String(parts[0]);
            var hash = Convert.FromBase64String(parts[1]);

            var hashToCompare = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, _hashAlgorithmName, KeySize);

            return CryptographicOperations.FixedTimeEquals(hash, hashToCompare);
        }

        public async Task<string> GenerateJwtTokenAsync(DiamondShopSystem.DAL.Entities.User user)
        {
            var userResponse = await _userService.GetUserByIdAsync(user.Id);
            string role = userResponse.Success ? userResponse.User.RoleName : "Customer";
            return _jwtUtil.GenerateToken(user, role);
        }
    }
} 