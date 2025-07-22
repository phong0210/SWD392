using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using DiamondShopSystem.DAL.Repositories;
using DiamondShopSystem.BLL.Services.Otp;
using DiamondShopSystem.BLL.Services.Email;
using DiamondShopSystem.BLL.Handlers.Auth.Commands.SendOtp;
using DiamondShopSystem.BLL.Services.Cache;

namespace DiamondShopSystem.BLL.Handlers.Auth.Handlers
{
    public class SendOtpCommandHandler : IRequestHandler<SendOtpCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOtpService _otpService;
        private readonly IEmailService _emailService;
        private readonly IOtpCacheService _otpCacheService;

        public SendOtpCommandHandler(IUnitOfWork unitOfWork, IOtpService otpService, IEmailService emailService, IOtpCacheService otpCacheService)
        {
            _unitOfWork = unitOfWork;
            _otpService = otpService;
            _emailService = emailService;
            _otpCacheService = otpCacheService;
        }

        public async Task<Unit> Handle(SendOtpCommand request, CancellationToken cancellationToken)
        {
            var userRepo = _unitOfWork.Repository<DiamondShopSystem.DAL.Entities.User>();
            var users = await userRepo.FindAsync(u => u.Email == request.Dto.Email);
            var user = users.FirstOrDefault();

            if (user != null)
            {
                var otp = _otpService.GenerateOtp();
                _otpCacheService.StoreOtp(user.Email, otp, DateTime.UtcNow.AddMinutes(10)); // OTP is valid for 10 minutes

                await _emailService.SendOtpEmailAsync(user.Email, otp);
            }

            return Unit.Value;
        }
    }
}
