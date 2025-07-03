using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DiamondShopSystem.DAL;
using DiamondShopSystem.BLL.Services;

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

        public async Task<string?> LoginAsync(string email, string password)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
                return null;

            // TODO: Replace with proper password hashing/verification
            if (user.PasswordHash != password)
                return null;

            // No direct Role property; use default or extend as needed
            var token = _jwtUtil.GenerateJwtToken(user, "User");
            return token;
        }
    }
} 