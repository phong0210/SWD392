using MediatR;
using DiamondShopSystem.BLL.Application.DTOs;
using DiamondShopSystem.BLL.Application.Interfaces;
using DiamondShopSystem.BLL.Utils;
using DiamondShopSystem.BLL.Application.Services.Authentication;
using System.Threading;
using System.Threading.Tasks;
using System;
using BCrypt.Net;

namespace DiamondShopSystem.BLL.Application.Features.Users
{
    public class LoginQuery : IRequest<LoginResponse>
    {
        public LoginRequest Login { get; set; }
        public string IpAddress { get; set; } = string.Empty;
    }

    public class LoginQueryHandler : IRequestHandler<LoginQuery, LoginResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJWTUtil _jwtUtil;
        private readonly IAuthenticationLogger _authLogger;

        public LoginQueryHandler(IUnitOfWork unitOfWork, IJWTUtil jwtUtil, IAuthenticationLogger authLogger)
        {
            _unitOfWork = unitOfWork;
            _jwtUtil = jwtUtil;
            _authLogger = authLogger;
        }

        public async Task<LoginResponse> Handle(LoginQuery request, CancellationToken cancellationToken)
        {
            var user = (await _unitOfWork.Users.ListAllAsync()).FirstOrDefault(u => u.Email == request.Login.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Login.Password, user.PasswordHash))
            {
                _authLogger.LogLoginAttempt(request.Login.Email, request.IpAddress, false, "Invalid credentials");
                return null; // Or throw an UnauthorizedException
            }

            var role = await _unitOfWork.Roles.GetByIdAsync(user.RoleId);

            var token = _jwtUtil.GenerateJwtToken(user, null, false);

            _authLogger.LogLoginAttempt(request.Login.Email, request.IpAddress, true);

            return new LoginResponse(
                token,
                user.Id,
                user.FullName,
                user.Email,
                role?.Name
            );
        }
    }
}