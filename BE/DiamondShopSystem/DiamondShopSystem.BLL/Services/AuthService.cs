using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DiamondShopSystem.DAL;
using DiamondShopSystem.DAL.Entities;
using DiamondShopSystem.BLL.Services;
using System.Security.Cryptography;
using System.Text;

namespace DiamondShopSystem.BLL.Services
{
    public class AuthService
    {
        private readonly AppDbContext _dbContext;
        private readonly JwtUtil _jwtUtil;

        public AuthService(AppDbContext dbContext, JwtUtil jwtUtil)
        {
            _dbContext = dbContext;
            _jwtUtil = jwtUtil;
        }

        public async Task<AuthResult> AuthenticateUserAsync(string email, string password)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
                return AuthResult.Failure("Invalid credentials");

            if (!VerifyPassword(password, user.PasswordHash))
                return AuthResult.Failure("Invalid credentials");

            var token = _jwtUtil.GenerateJwtToken(user, "User");
            return AuthResult.Success(token, user);
        }

        private bool VerifyPassword(string password, string storedHash)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(bytes);
                var hashString = Convert.ToBase64String(hash);
                return hashString == storedHash;
            }
        }
    }

    public class AuthResult
    {
        public bool IsSuccess { get; private set; }
        public string Token { get; private set; } = string.Empty;
        public User User { get; private set; } = null!;
        public string ErrorMessage { get; private set; } = string.Empty;

        public static AuthResult Success(string token, User user)
        {
            return new AuthResult
            {
                IsSuccess = true,
                Token = token,
                User = user
            };
        }

        public static AuthResult Failure(string errorMessage)
        {
            return new AuthResult
            {
                IsSuccess = false,
                ErrorMessage = errorMessage
            };
        }
    }
} 