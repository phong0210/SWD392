using MediatR;
using System.Threading;
using System.Threading.Tasks;
using DiamondShopSystem.BLL.Handlers.Auth.DTOs;
using DiamondShopSystem.BLL.Services.Auth;
using DiamondShopSystem.DAL.Repositories;
using Microsoft.Extensions.Configuration;
using Google.Apis.Auth;
using System.Linq;
using System;
using DiamondShopSystem.BLL.Handlers.User.DTOs;

namespace DiamondShopSystem.BLL.Handlers.Auth.Commands.GoogleLogin
{
    public class GoogleLoginCommandHandler : IRequestHandler<GoogleLoginCommand, LoginResponseDto>
    {
        private readonly IGenericRepository<DiamondShopSystem.DAL.Entities.User> _userRepository;
        private readonly IAuthService _authService;
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;

        public GoogleLoginCommandHandler(IGenericRepository<DiamondShopSystem.DAL.Entities.User> userRepository, IAuthService authService, IConfiguration configuration, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _authService = authService;
            _configuration = configuration;
            _unitOfWork = unitOfWork;
        }

        public async Task<LoginResponseDto> Handle(GoogleLoginCommand request, CancellationToken cancellationToken)
        {
            var clientId = _configuration["Google:ClientId"];
            if (string.IsNullOrEmpty(clientId))
                throw new InvalidOperationException("Google Client ID is not set in environment variables or configuration");

            var validationSettings = new GoogleJsonWebSignature.ValidationSettings
            {
                Audience = new[] { clientId }
            };

            var payload = await GoogleJsonWebSignature.ValidateAsync(request.GoogleLoginRequestDto.Credential, validationSettings);

            var users = await _userRepository.FindAsync(u => u.Email == payload.Email);
            var user = users.FirstOrDefault();

            if (user == null)
            {
                user = new DiamondShopSystem.DAL.Entities.User
                {
                    Email = payload.Email,
                    Name = payload.Name,
                    PasswordHash = _authService.HashPassword(Guid.NewGuid().ToString()), // Generate a random password
                    Status = true,
                    CreatedAt = DateTime.UtcNow
                };
                await _userRepository.AddAsync(user);
            }
            else
            {
                user.Name = payload.Name;
                _userRepository.Update(user);
            }

            await _unitOfWork.SaveChangesAsync();

            var token = await _authService.GenerateJwtTokenAsync(user);

            return new LoginResponseDto
            {
                Token = token,
                Email = user.Email,
                Name = user.Name
            };
        }
    }
}