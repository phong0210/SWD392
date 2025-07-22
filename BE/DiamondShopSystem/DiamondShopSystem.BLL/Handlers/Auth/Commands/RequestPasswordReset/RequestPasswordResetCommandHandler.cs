using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using DiamondShopSystem.DAL.Repositories;
using DiamondShopSystem.BLL.Services.Otp;
using DiamondShopSystem.BLL.Services.Email;
using DiamondShopSystem.BLL.Services.Cache;

namespace DiamondShopSystem.BLL.Handlers.Auth.Commands.RequestPasswordReset
{
    public class RequestPasswordResetCommandHandler : IRequestHandler<RequestPasswordResetCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOtpService _otpService;
        private readonly IEmailService _emailService;
        private readonly IOtpCacheService _otpCacheService;

        public RequestPasswordResetCommandHandler(IUnitOfWork unitOfWork, IOtpService otpService, IEmailService emailService, IOtpCacheService otpCacheService)
        {
            _unitOfWork = unitOfWork;
            _otpService = otpService;
            _emailService = emailService;
            _otpCacheService = otpCacheService;
        }

        public async Task<bool> Handle(RequestPasswordResetCommand request, CancellationToken cancellationToken)
        {
            var userRepo = _unitOfWork.Repository<DiamondShopSystem.DAL.Entities.User>();
            var users = await userRepo.FindAsync(u => u.Email == request.Dto.Email);
            var user = users.FirstOrDefault();

            if (user != null)
            {
                var otp = _otpService.GenerateOtp();
                _otpCacheService.StoreOtp(user.Email, otp, DateTime.UtcNow.AddMinutes(10)); // OTP is valid for 10 minutes

                await _emailService.SendOtpEmailAsync(user.Email, otp);
                return true;
            }
            // If the user is not found, we don't throw an error to prevent email enumeration attacks.
            // The request will complete silently, but return false.
            return false;
        }
    }
}
