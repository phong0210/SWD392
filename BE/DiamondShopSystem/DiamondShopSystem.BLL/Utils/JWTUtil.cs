using DiamondShopSystem.BLL.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DiamondShopSystem.BLL.Utils
{
    public class JwtUtil : IJWTUtil
    {
        private readonly string _jwtKey, _issuer, _audience;
        private readonly double _tokenValidityInMinutes;

        public JwtUtil(IConfiguration configuration)
        {
            _jwtKey = configuration["Jwt:Key"];
            _issuer = configuration["Jwt:Issuer"];
            _audience = configuration["Jwt:Audience"];
            _tokenValidityInMinutes = double.Parse(configuration["Jwt:TokenValidityInMinutes"] ?? "30");
        }

        public string GenerateJwtToken(User user, Tuple<string, Guid>? guidClaimer, bool isResetPasswordOnly)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtKey));
            var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
                    {
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Sub, user.Email ?? string.Empty),
                        new Claim(ClaimTypes.Role, user.Role?.Name ?? "User") 
                    };

            if (guidClaimer != null)
            {
                claims.Add(new Claim(guidClaimer.Item1, guidClaimer.Item2.ToString()));
            }

            if (isResetPasswordOnly)
            {
                claims.Add(new Claim("ResetPasswordOnly", "true"));
            }

            var expires = DateTime.UtcNow.AddMinutes(_tokenValidityInMinutes);

            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: expires,
                signingCredentials: credentials
            );

            return tokenHandler.WriteToken(token);
        }
    }
}
