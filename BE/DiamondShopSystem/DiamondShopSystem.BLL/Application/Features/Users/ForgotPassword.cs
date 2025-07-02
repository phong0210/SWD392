using MediatR;
using DiamondShopSystem.BLL.Application.DTOs;
using DiamondShopSystem.BLL.Application.Interfaces;
using DiamondShopSystem.BLL.Utils;
using DiamondShopSystem.BLL.Application.Services.Authentication;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace DiamondShopSystem.BLL.Application.Features.Users
{
    public class ForgotPasswordCommand : IRequest<ForgotPasswordResponse>
    {
        public ForgotPasswordRequest Request { get; set; }
        public string IpAddress { get; set; } = string.Empty;
    }

    public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand, ForgotPasswordResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJWTUtil _jwtUtil;
        private readonly IOTPUtil _otpUtil;
        private readonly IAuthenticationLogger _authLogger;

        public ForgotPasswordCommandHandler(IUnitOfWork unitOfWork, IJWTUtil jwtUtil, IOTPUtil otpUtil, IAuthenticationLogger authLogger)
        {
            _unitOfWork = unitOfWork;
            _jwtUtil = jwtUtil;
            _otpUtil = otpUtil;
            _authLogger = authLogger;
        }

        public async Task<ForgotPasswordResponse> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = (await _unitOfWork.Users.ListAllAsync()).FirstOrDefault(u => u.Email == request.Request.Email);

            if (user == null)
            {
                // For security reasons, don't reveal if the email exists or not
                _authLogger.LogPasswordResetRequest(request.Request.Email, request.IpAddress, true, "Email not found (not revealed to user)");
                return new ForgotPasswordResponse(
                    "If the email address exists in our system, you will receive a password reset link shortly.",
                    true
                );
            }

            // Generate OTP for additional security
            var otp = _otpUtil.GenerateOtp(user.Email);

            // Generate password reset token (valid for 15 minutes)
            var resetToken = _jwtUtil.GenerateJwtToken(user, null, true);

            // In a real application, you would:
            // 1. Store the OTP in a temporary storage (Redis, database, etc.)
            // 2. Send email with reset link containing token and OTP
            // 3. Set expiration for the OTP

            // For now, we'll just return success message
            // TODO: Implement email service to send reset link
            _authLogger.LogPasswordResetRequest(request.Request.Email, request.IpAddress, true);
            
            return new ForgotPasswordResponse(
                "If the email address exists in our system, you will receive a password reset link shortly.",
                true
            );
        }
    }
} 