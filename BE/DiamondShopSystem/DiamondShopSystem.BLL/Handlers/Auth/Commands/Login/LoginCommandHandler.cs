using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using MediatR;
using DiamondShopSystem.DAL.Repositories;
using DiamondShopSystem.DAL.Entities;
using DiamondShopSystem.BLL.Handlers.Auth.DTOs;
using DiamondShopSystem.BLL.Services.Auth;

namespace DiamondShopSystem.BLL.Handlers.Auth.Commands.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponseDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthService _authService;

        public LoginCommandHandler(IUnitOfWork unitOfWork, IAuthService authService)
        {
            _unitOfWork = unitOfWork;
            _authService = authService;
        }

        public async Task<LoginResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var userRepo = _unitOfWork.Repository<DiamondShopSystem.DAL.Entities.User>();
            var user = await userRepo.FindAsync(u => u.Email == request.Request.Email);
            var userEntity = user.FirstOrDefault();

            if (userEntity == null)
            {
                return new LoginResponseDto();
            }

            var hashedPassword = _authService.HashPassword(request.Request.Password);
            if (userEntity.PasswordHash != hashedPassword)
            {
                return new LoginResponseDto();
            }

            var token = _authService.GenerateJwtTokenAsync(userEntity);
            return new LoginResponseDto
            {
                Token = await token,
                Email = userEntity.Email,
                Name = userEntity.Name
            };
        }
    }
} 