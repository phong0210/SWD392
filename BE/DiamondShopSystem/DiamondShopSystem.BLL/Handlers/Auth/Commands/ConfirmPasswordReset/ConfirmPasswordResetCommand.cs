using MediatR;
using DiamondShopSystem.BLL.Handlers.Auth.DTOs;

namespace DiamondShopSystem.BLL.Handlers.Auth.Commands.ConfirmPasswordReset
{
    public class ConfirmPasswordResetCommand : IRequest<bool>
    {
        public ConfirmPasswordResetDto Dto { get; }

        public ConfirmPasswordResetCommand(ConfirmPasswordResetDto dto)
        {
            Dto = dto;
        }
    }
}
