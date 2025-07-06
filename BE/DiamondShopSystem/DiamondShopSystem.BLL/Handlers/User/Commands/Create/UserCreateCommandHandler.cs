using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using MediatR;
using AutoMapper;
using DiamondShopSystem.DAL.Repositories;
using DiamondShopSystem.DAL.Entities;
using DiamondShopSystem.BLL.Handlers.User.DTOs;
using DiamondShopSystem.BLL.Services;

namespace DiamondShopSystem.BLL.Handlers.User.Commands.Create
{
    public class UserCreateCommandHandler : IRequestHandler<UserCreateCommand, UserCreateResponseDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserCreateCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
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
            user.PasswordHash = HashPassword(request.Dto.Password);
            await userRepo.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();
            return new UserCreateResponseDto {
                Success = true,
                UserId = user.Id,
                Email = user.Email
            };
        }

        private string HashPassword(string password)
        {
            using var sha256 = System.Security.Cryptography.SHA256.Create();
            var hashedBytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }
    }
} 