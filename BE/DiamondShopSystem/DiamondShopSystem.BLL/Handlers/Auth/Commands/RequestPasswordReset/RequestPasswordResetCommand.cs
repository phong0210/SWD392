using MediatR;
using DiamondShopSystem.BLL.Handlers.Auth.DTOs;

namespace DiamondShopSystem.BLL.Handlers.Auth.Commands.RequestPasswordReset
{
    public class RequestPasswordResetCommand : IRequest<bool>
    {
        public RequestPasswordResetDto Dto { get; }

        public RequestPasswordResetCommand(RequestPasswordResetDto dto)
        {
            Dto = dto;
        }
    }
}
