using MediatR;
using DiamondShopSystem.BLL.Application.DTOs;
using DiamondShopSystem.BLL.Application.Interfaces;
using DiamondShopSystem.BLL.Utils;
using DiamondShopSystem.BLL.Application.Services.Authentication;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using BCrypt.Net;
using Microsoft.Extensions.Configuration;

namespace DiamondShopSystem.BLL.Application.Features.Users
{
    public class ResetPasswordCommand : IRequest<ResetPasswordResponse>
    {
        public ResetPasswordRequest Request { get; set; }
        public string IpAddress { get; set; } = string.Empty;
    }

    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, ResetPasswordResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly IAuthenticationLogger _authLogger;

        public ResetPasswordCommandHandler(IUnitOfWork unitOfWork, IConfiguration configuration, IAuthenticationLogger authLogger)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _authLogger = authLogger;
        }

        public async Task<ResetPasswordResponse> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Validate and decode the JWT token
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtKey = _configuration["Jwt:Key"];
                var issuer = _configuration["Jwt:Issuer"];
                var audience = _configuration["Jwt:Audience"];

                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey ?? string.Empty)),
                    ValidateIssuer = true,
                    ValidIssuer = issuer,
                    ValidateAudience = true,
                    ValidAudience = audience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };

                var principal = tokenHandler.ValidateToken(request.Request.Token, validationParameters, out var validatedToken);
                var jwtToken = (JwtSecurityToken)validatedToken;

                // Check if this is a password reset token
                var resetPasswordOnly = principal.Claims.FirstOrDefault(c => c.Type == "ResetPasswordOnly")?.Value;
                if (resetPasswordOnly != "true")
                {
                    _authLogger.LogPasswordReset("unknown", request.IpAddress, false, "Invalid reset token type");
                    return new ResetPasswordResponse("Invalid reset token.", false);
                }

                // Get user email from token
                var email = principal.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;
                if (string.IsNullOrEmpty(email))
                {
                    _authLogger.LogPasswordReset("unknown", request.IpAddress, false, "No email in token");
                    return new ResetPasswordResponse("Invalid reset token.", false);
                }

                // Find user by email
                var user = (await _unitOfWork.Users.ListAllAsync()).FirstOrDefault(u => u.Email == email);
                if (user == null)
                {
                    _authLogger.LogPasswordReset(email, request.IpAddress, false, "User not found");
                    return new ResetPasswordResponse("User not found.", false);
                }

                // Hash the new password
                var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Request.NewPassword);
                user.PasswordHash = hashedPassword;

                // Update user in database (no explicit Update needed)
                await _unitOfWork.CommitAsync();

                _authLogger.LogPasswordReset(email, request.IpAddress, true);

                return new ResetPasswordResponse("Password has been reset successfully.", true);
            }
            catch (SecurityTokenExpiredException)
            {
                _authLogger.LogPasswordReset("unknown", request.IpAddress, false, "Token expired");
                return new ResetPasswordResponse("Reset token has expired. Please request a new one.", false);
            }
            catch (SecurityTokenInvalidSignatureException)
            {
                _authLogger.LogPasswordReset("unknown", request.IpAddress, false, "Invalid token signature");
                return new ResetPasswordResponse("Invalid reset token.", false);
            }
            catch (Exception ex)
            {
                _authLogger.LogPasswordReset("unknown", request.IpAddress, false, $"Exception: {ex.Message}");
                return new ResetPasswordResponse("An error occurred while resetting the password.", false);
            }
        }
    }
} 