using System.Threading;
using System.Threading.Tasks;
using MediatR;
using AutoMapper;
using DiamondShopSystem.DAL.Repositories;
using DiamondShopSystem.DAL.Entities;
using DiamondShopSystem.BLL.Services.Auth;
using DiamondShopSystem.BLL.Services.Cache;
using DiamondShopSystem.BLL.Handlers.User.DTOs;

namespace DiamondShopSystem.BLL.Handlers.User.Commands.ConfirmRegistration
{
    public class ConfirmRegistrationCommandHandler : IRequestHandler<ConfirmRegistrationCommand, UserRegisterResponseDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;
        private readonly IOtpCacheService _otpCacheService;

        public ConfirmRegistrationCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IAuthService authService, IOtpCacheService otpCacheService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _authService = authService;
            _otpCacheService = otpCacheService;
        }

        public async Task<UserRegisterResponseDto> Handle(ConfirmRegistrationCommand request, CancellationToken cancellationToken)
        {
            var storedOtpData = _otpCacheService.GetOtp(request.Dto.Email);
            if (storedOtpData == null || storedOtpData.Value.otp != request.Dto.Otp || storedOtpData.Value.expiration <= System.DateTime.UtcNow)
            {
                return new UserRegisterResponseDto
                {
                    Success = false,
                    Error = "Invalid or expired OTP.",
                    Email = request.Dto.Email
                };
            }

            var userRegisterDto = _otpCacheService.GetUserRegistrationData(request.Dto.Email);
            if (userRegisterDto == null)
            {
                return new UserRegisterResponseDto
                {
                    Success = false,
                    Error = "Registration data not found. Please try registering again.",
                    Email = request.Dto.Email
                };
            }

            var userRepo = _unitOfWork.Repository<DiamondShopSystem.DAL.Entities.User>();
            var user = _mapper.Map<DiamondShopSystem.DAL.Entities.User>(userRegisterDto);
            user.PasswordHash = _authService.HashPassword(userRegisterDto.Password);

            await userRepo.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();

            var loyaltyPoint = new DiamondShopSystem.DAL.Entities.LoyaltyPoints
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                PointsEarned = 0,
                PointsRedeemed = 0,
                LastUpdated = DateTime.UtcNow
            };

            await _unitOfWork.Repository<DiamondShopSystem.DAL.Entities.LoyaltyPoints>().AddAsync(loyaltyPoint);
            await _unitOfWork.SaveChangesAsync();

            _otpCacheService.RemoveOtp(request.Dto.Email);
            _otpCacheService.RemoveUserRegistrationData(request.Dto.Email);

            return new UserRegisterResponseDto
            {
                Success = true,
                Email = user.Email,
                Error = "User registered successfully."
            };
        }
    }
}