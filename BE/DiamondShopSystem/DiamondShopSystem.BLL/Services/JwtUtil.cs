using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using DiamondShopSystem.DAL.Entities;

namespace DiamondShopSystem.BLL.Services
{
    public class JwtUtil
    {
        private readonly string _key;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly int _tokenValidityInMinutes;
        private readonly int _refreshTokenValidityDays;

        public JwtUtil(IConfiguration configuration)
        {
            _key = Environment.GetEnvironmentVariable("JWT_KEY") ?? throw new InvalidOperationException("JWT_KEY is not set");
            _issuer = Environment.GetEnvironmentVariable("JWT_ISSUER") ?? throw new InvalidOperationException("JWT_ISSUER is not set");
            _audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE") ?? throw new InvalidOperationException("JWT_AUDIENCE is not set");
            _tokenValidityInMinutes = int.TryParse(Environment.GetEnvironmentVariable("JWT_TOKEN_VALIDITY_IN_MINUTES"), out var min) ? min : 30;
            _refreshTokenValidityDays = int.TryParse(Environment.GetEnvironmentVariable("JWT_REFRESH_TOKEN_VALIDITY_IN_DAYS"), out var days) ? days : 7;
        }

        public string GenerateJwtToken(User user, string role)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim("AccountID", user.Id.ToString()),
                new Claim(ClaimTypes.Role, role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_tokenValidityInMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
} 