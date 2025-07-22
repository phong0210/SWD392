using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using DiamondShopSystem.BLL.Services.Cache;

namespace DiamondShopSystem.BLL.Handlers.Auth.Commands.VerifyOtp
{
    public class VerifyOtpCommandHandler : IRequestHandler<VerifyOtpCommand, bool>
    {
        private readonly IOtpCacheService _otpCacheService;

        public VerifyOtpCommandHandler(IOtpCacheService otpCacheService)
        {
            _otpCacheService = otpCacheService;
        }

        public async Task<bool> Handle(VerifyOtpCommand request, CancellationToken cancellationToken)
        {
            var storedOtpData = _otpCacheService.GetOtp(request.Dto.Email);

            if (storedOtpData == null)
            {
                return false; // OTP not found or expired
            }

            var (storedOtp, expiration) = storedOtpData.Value;

            if (storedOtp == request.Dto.Otp && expiration > DateTime.UtcNow)
            {
                _otpCacheService.RemoveOtp(request.Dto.Email); // OTP successfully verified, remove it
                return true;
            }

            return false; 
        }
    }
}
