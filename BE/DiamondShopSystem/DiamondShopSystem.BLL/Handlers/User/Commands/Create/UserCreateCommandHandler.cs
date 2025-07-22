using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using DiamondShopSystem.BLL.Services.Auth;
using MediatR;
using AutoMapper;
using DiamondShopSystem.DAL.Repositories;
using DiamondShopSystem.DAL.Entities;
using DiamondShopSystem.BLL.Handlers.User.DTOs;

namespace DiamondShopSystem.BLL.Handlers.User.Commands.Create
{
    public class UserCreateCommandHandler : IRequestHandler<UserCreateCommand, UserCreateResponseDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public UserCreateCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IAuthService authService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<UserCreateResponseDto> Handle(UserCreateCommand request, CancellationToken cancellationToken)
        {
            var userRepo = _unitOfWork.Repository<DiamondShopSystem.DAL.Entities.User>();
            var existing = await userRepo.FindAsync(u => u.Email == request.Dto.Email);
            if (existing.Any())
            {
                return new UserCreateResponseDto {
                    Success = false,
                    Error = "Email already exists.",
                    Email = request.Dto.Email
                };
            }
            var user = _mapper.Map<DiamondShopSystem.DAL.Entities.User>(request.Dto);
            user.PasswordHash = _authService.HashPassword(request.Dto.Password);
            await userRepo.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();
            return new UserCreateResponseDto {
                Success = true,
                UserId = user.Id,
                Email = user.Email
            };
        }
    }
} 