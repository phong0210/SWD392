using DiamondShopSystem.BLL.Application.DTOs;
using DiamondShopSystem.BLL.Application.Interfaces;
using DiamondShopSystem.BLL.Domain.Entities;
using DiamondShopSystem.BLL.Utils;
using System;
using System.Linq;
using System.Threading.Tasks;
using BCrypt.Net;

namespace DiamondShopSystem.BLL.Application.Services.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJWTUtil _jwtUtil;

        public AuthenticationService(IUnitOfWork unitOfWork, IJWTUtil jwtUtil)
        {
            _unitOfWork = unitOfWork;
            _jwtUtil = jwtUtil;
        }

        public async Task<LoginResponse> AuthenticateAsync(LoginRequest request)
        {
            var user = (await _unitOfWork.Users.ListAllAsync()).FirstOrDefault(u => u.Email == request.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                return null; // Or throw an UnauthorizedException
            }

            var role = await _unitOfWork.Roles.GetByIdAsync(user.RoleId);

            var token = _jwtUtil.GenerateJwtToken(user, null, false);

            return new LoginResponse(
                token,
                user.Id,
                user.FullName,
                user.Email,
                role?.Name
            );
        }

        public async Task<LoginResponse> GoogleAuthenticateAsync(string idToken)
        {
            // This is a placeholder. Real implementation would involve:
            // 1. Calling Google's API to validate the idToken and get user info.
            // 2. Checking if the user exists in your database.
            // 3. If not, register the user (e.g., create a new User and CustomerProfile).
            // 4. Generate and return a JWT for the user.
            throw new NotImplementedException("Google authentication is not yet implemented.");
        }
    }
}