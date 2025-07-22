using MediatR;
using DiamondShopSystem.BLL.Handlers.Auth.DTOs;

namespace DiamondShopSystem.BLL.Handlers.Auth.Commands.SendOtp
{
    public class SendOtpCommand : IRequest<Unit>
    {
        public SendOtpRequestDto Dto { get; }

        public SendOtpCommand(SendOtpRequestDto dto)
        {
            Dto = dto;
        }
    }
}
