using DiamondShopSystem.BLL.Handlers.User.DTOs;
using MediatR;

namespace DiamondShopSystem.BLL.Handlers.User.Commands.Deactivate
{
    public class DeactivateUserCommand : IRequest<UserUpdateResponseDto>
    {
        public Guid UserId { get; }

        public DeactivateUserCommand(Guid userId)
        {
            UserId = userId;
        }
    }
}
