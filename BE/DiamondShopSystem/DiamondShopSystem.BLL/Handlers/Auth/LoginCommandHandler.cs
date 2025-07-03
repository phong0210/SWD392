using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using MediatR;
using DiamondShopSystem.DAL.Repositories;
using DiamondShopSystem.BLL.Services;
using DiamondShopSystem.DAL.Entities;

namespace DiamondShopSystem.BLL.Handlers.Auth
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponseDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly JwtUtil _jwtUtil;
        private readonly IMapper _mapper;

        public LoginCommandHandler(IUnitOfWork unitOfWork, JwtUtil jwtUtil, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _jwtUtil = jwtUtil;
            _mapper = mapper;
        }

        public async Task<LoginResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var userRepo = _unitOfWork.Repository<DiamondShopSystem.DAL.Entities.User>();
            var users = await userRepo.FindAsync(u => u.Email == request.Request.Email);
            var foundUser = users.FirstOrDefault();
            if (foundUser == null || !VerifyPassword(request.Request.Password, foundUser.PasswordHash))
            {
                return null;
            }
            var token = _jwtUtil.GenerateJwtToken(foundUser, "User");
            return new LoginResponseDto { Token = token };
        }

        // Use the same hash function as in user creation
        private bool VerifyPassword(string password, string storedHash)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(bytes);
                var hashString = Convert.ToBase64String(hash);
                return hashString == storedHash;
            }
        }
    }
} 