using MediatR;
using DiamondShopSystem.BLL.Application.DTOs;
using DiamondShopSystem.BLL.Application.Services.Authentication;
using DiamondShopSystem.BLL.Application.Interfaces;
using DiamondShopSystem.BLL.Utils;
using System.Threading;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace DiamondShopSystem.BLL.Application.Features.Users
{
    public class GoogleLoginQuery : IRequest<LoginResponse>
    {
        public string IdToken { get; set; } = string.Empty;
    }

    public class GoogleLoginQueryHandler : IRequestHandler<GoogleLoginQuery, LoginResponse>
    {
        private readonly IGoogleOAuthService _googleOAuthService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJWTUtil _jwtUtil;

        public GoogleLoginQueryHandler(IGoogleOAuthService googleOAuthService, IUnitOfWork unitOfWork, IJWTUtil jwtUtil)
        {
            _googleOAuthService = googleOAuthService;
            _unitOfWork = unitOfWork;
            _jwtUtil = jwtUtil;
        }

        public async Task<LoginResponse> Handle(GoogleLoginQuery request, CancellationToken cancellationToken)
        {
            var userInfo = await _googleOAuthService.ValidateIdTokenAsync(request.IdToken);
            if (userInfo == null || !userInfo.EmailVerified)
                return null!;

            // Check if user exists
            var user = (await _unitOfWork.Users.ListAllAsync()).FirstOrDefault(u => u.Email == userInfo.Email);
            if (user == null)
            {
                // Register new user as Customer
                var customerRole = (await _unitOfWork.Roles.ListAllAsync()).FirstOrDefault(r => r.Name == "Customer");
                user = new BLL.Domain.Entities.User
                {
                    Id = Guid.NewGuid(),
                    FullName = userInfo.FullName,
                    Email = userInfo.Email,
                    Phone = string.Empty,
                    PasswordHash = string.Empty, // No password for Google login
                    RoleId = customerRole != null ? customerRole.Id : Guid.Empty,
                    IsActive = true
                };
                await _unitOfWork.Users.AddAsync(user);
                await _unitOfWork.CommitAsync();
            }

            // Generate JWT
            var token = _jwtUtil.GenerateJwtToken(user, null, false);
            return new LoginResponse(
                token,
                user.Id,
                user.FullName,
                user.Email,
                (await _unitOfWork.Roles.GetByIdAsync(user.RoleId))?.Name ?? "Customer"
            );
        }
    }
} 