using MediatR;
using DiamondShopSystem.BLL.Handlers.User.DTOs;

namespace DiamondShopSystem.BLL.Handlers.User.Commands.Update
{
    public class UserUpdateCommand : IRequest<UserUpdateResponseDto>
    {
        public Guid UserId { get; }
        public UserUpdateDto UpdateData { get; }

        public UserUpdateCommand(Guid userId, UserUpdateDto updateData)
        {
            UserId = userId;
            UpdateData = updateData;
        }
    }
} 