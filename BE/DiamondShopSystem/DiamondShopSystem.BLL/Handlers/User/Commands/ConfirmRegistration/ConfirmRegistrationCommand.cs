using MediatR;
using DiamondShopSystem.BLL.Handlers.User.DTOs;

namespace DiamondShopSystem.BLL.Handlers.User.Commands.ConfirmRegistration
{
    public class ConfirmRegistrationCommand : IRequest<UserRegisterResponseDto>
    {
        public ConfirmRegistrationDto Dto { get; }

        public ConfirmRegistrationCommand(ConfirmRegistrationDto dto)
        {
            Dto = dto;
        }
    }
}