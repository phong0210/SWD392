using MediatR;
using DiamondShopSystem.BLL.Handlers.Auth.DTOs;

namespace DiamondShopSystem.BLL.Handlers.Auth.Commands.VerifyOtp
{
    public class VerifyOtpCommand : IRequest<bool>
    {
        public VerifyOtpRequestDto Dto { get; }

        public VerifyOtpCommand(VerifyOtpRequestDto dto)
        {
            Dto = dto;
        }
    }
}
