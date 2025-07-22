using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using DiamondShopSystem.DAL.Repositories;
using DiamondShopSystem.BLL.Services.Auth;
using DiamondShopSystem.BLL.Services.Cache;

namespace DiamondShopSystem.BLL.Handlers.Auth.Commands.ConfirmPasswordReset
{
    public class ConfirmPasswordResetCommandHandler : IRequestHandler<ConfirmPasswordResetCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthService _authService;
        private readonly IOtpCacheService _otpCacheService;

        public ConfirmPasswordResetCommandHandler(IUnitOfWork unitOfWork, IAuthService authService, IOtpCacheService otpCacheService)
        {
            _unitOfWork = unitOfWork;
            _authService = authService;
            _otpCacheService = otpCacheService;
        }

        public async Task<bool> Handle(ConfirmPasswordResetCommand request, CancellationToken cancellationToken)
        {
            var storedOtpData = _otpCacheService.GetOtp(request.Dto.Email);

            if (storedOtpData == null || storedOtpData.Value.otp != request.Dto.Otp || storedOtpData.Value.expiration <= DateTime.UtcNow)
            {
                return false; // OTP not found, mismatch, or expired
            }

            var userRepo = _unitOfWork.Repository<DiamondShopSystem.DAL.Entities.User>();
            var users = await userRepo.FindAsync(u => u.Email == request.Dto.Email);
            var user = users.FirstOrDefault();

            if (user == null)
            {
                return false; // User not found
            }

            user.PasswordHash = _authService.HashPassword(request.Dto.NewPassword);
            userRepo.Update(user);
            await _unitOfWork.SaveChangesAsync();

            _otpCacheService.RemoveOtp(request.Dto.Email); // OTP successfully used, remove it

            return true;
        }
    }
}
