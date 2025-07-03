using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using MediatR;
using AutoMapper;
using DiamondShopSystem.DAL.Repositories;
using DiamondShopSystem.DAL.Entities;
using System.Security.Cryptography;
using System.Text;

namespace DiamondShopSystem.BLL.Handlers.User
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

        // Simple SHA256 hash for demonstration (replace with BCrypt or similar in production)
        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }
    }
} 