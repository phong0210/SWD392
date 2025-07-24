using MediatR;
using System;
using DiamondShopSystem.BLL.Handlers.User.DTOs;

namespace DiamondShopSystem.BLL.Handlers.User.Commands.RemoveStaffRole
{
    public class RemoveStaffRoleCommand : IRequest<UserUpdateResponseDto>
    {
        public Guid UserId { get; }

        public RemoveStaffRoleCommand(Guid userId)
        {
            UserId = userId;
        }
    }
}
