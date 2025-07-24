using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using MediatR;
using AutoMapper;
using DiamondShopSystem.DAL.Repositories;
using DiamondShopSystem.BLL.Services.Auth;
using DiamondShopSystem.BLL.Services.Otp;
using DiamondShopSystem.BLL.Services.Email;
using DiamondShopSystem.BLL.Services.Cache;
using DiamondShopSystem.BLL.Handlers.User.DTOs;

namespace DiamondShopSystem.BLL.Handlers.User.Commands.Register
{
    public class UserRegisterCommandHandler : IRequestHandler<UserRegisterCommand, UserRegisterResponseDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;
        private readonly IOtpService _otpService;
        private readonly IEmailService _emailService;
        private readonly IOtpCacheService _otpCacheService;

        public UserRegisterCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IAuthService authService, IOtpService otpService, IEmailService emailService, IOtpCacheService otpCacheService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _authService = authService;
            _otpService = otpService;
            _emailService = emailService;
            _otpCacheService = otpCacheService;
        }

        public async Task<UserRegisterResponseDto> Handle(UserRegisterCommand request, CancellationToken cancellationToken)
        {
            var userRepo = _unitOfWork.Repository<DiamondShopSystem.DAL.Entities.User>();
            var existing = await userRepo.FindAsync(u => u.Email == request.Dto.Email);
            if (existing.Any())
            {
                return new UserRegisterResponseDto
                {
                    Success = false,
                    Error = "Email already exists.",
                    Email = request.Dto.Email
                };
            }

            var otp = _otpService.GenerateOtp();
            // Store the registration details temporarily with the OTP
            _otpCacheService.StoreOtp(request.Dto.Email, otp, System.DateTime.UtcNow.AddMinutes(10)); // OTP valid for 10 minutes
            // Store the user DTO temporarily as well, associated with the email
            _otpCacheService.StoreUserRegistrationData(request.Dto.Email, request.Dto);

            await _emailService.SendOtpEmailAsync(request.Dto.Email, otp);

            return new UserRegisterResponseDto
            {
                Success = true, 
                Email = request.Dto.Email,
                Error = "OTP sent to email. Please verify to complete registration."
            };
        }
    }
}