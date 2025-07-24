using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using MediatR;
using DiamondShopSystem.DAL.Repositories;
using DiamondShopSystem.BLL.Handlers.Auth.DTOs;
using DiamondShopSystem.BLL.Services.Auth;
using Microsoft.Extensions.Logging;

namespace DiamondShopSystem.BLL.Handlers.Auth.Commands.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponseDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthService _authService;
        private readonly ILogger<LoginCommandHandler> _logger;

        public LoginCommandHandler(IUnitOfWork unitOfWork, IAuthService authService, ILogger<LoginCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _authService = authService;
            _logger = logger;
        }

        public async Task<LoginResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Attempting to log in user with email: {Email}", request.Request.Email);

            var userRepo = _unitOfWork.Repository<DiamondShopSystem.DAL.Entities.User>();
            var user = await userRepo.FindAsync(u => u.Email == request.Request.Email);
            var userEntity = user.FirstOrDefault();

            if (userEntity == null)
            {
                _logger.LogWarning("User with email: {Email} not found.", request.Request.Email);
                return new LoginResponseDto { Error = "User not found." };
            }

            if (!userEntity.Status)
            {
                _logger.LogWarning("User with email: {Email} is inactive.", request.Request.Email);
                return new LoginResponseDto { Error = "User account is inactive." };
            }

            _logger.LogInformation("User found: {Email}. Validating password.", userEntity.Email);
            var isValidPassword = _authService.ValidatePassword(request.Request.Password, userEntity.PasswordHash);
            if (!isValidPassword)
            {
                _logger.LogWarning("Invalid password for user: {Email}", userEntity.Email);
                return new LoginResponseDto { Error = "Invalid password." };
            }

            _logger.LogInformation("Password validated for user: {Email}. Generating token.", userEntity.Email);
            var token = await _authService.GenerateJwtTokenAsync(userEntity);
            _logger.LogInformation("Token generated successfully for user: {Email}", userEntity.Email);

            return new LoginResponseDto
            {
                Token = token,
                Email = userEntity.Email,
                Name = userEntity.Name
            };
        }
    }
} 