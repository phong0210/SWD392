using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using DiamondShopSystem.DAL.Entities;

namespace DiamondShopSystem.BLL.Services.Auth
{
    public class JwtUtil
    {
        private readonly string _jwtKey;
        private readonly string _jwtIssuer;
        private readonly string _jwtAudience;
        private readonly int _jwtTokenValidityInMinutes;

        public JwtUtil()
        {
            _jwtKey = Environment.GetEnvironmentVariable("JWT_KEY") ?? throw new InvalidOperationException("JWT_KEY environment variable is not set");
            _jwtIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER") ?? throw new InvalidOperationException("JWT_ISSUER environment variable is not set");
            _jwtAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE") ?? throw new InvalidOperationException("JWT_AUDIENCE environment variable is not set");
            _jwtTokenValidityInMinutes = int.Parse(Environment.GetEnvironmentVariable("JWT_TOKEN_VALIDITY_IN_MINUTES") ?? "60");
        }

        public string GenerateToken(DiamondShopSystem.DAL.Entities.User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtKey);

            // Determine role based on StaffProfile
            string role = "Customer"; // Default role
            if (user.StaffProfile != null && user.StaffProfile.Role != null)
            {
                role = user.StaffProfile.Role.Name;
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim("AccountID", user.Id.ToString()),
                new Claim("Email", user.Email),
                new Claim("Role", role)
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_jwtTokenValidityInMinutes),
                Issuer = _jwtIssuer,
                Audience = _jwtAudience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
} 