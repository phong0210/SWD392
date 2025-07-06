using System;
using MediatR;
using DiamondShopSystem.BLL.Handlers.User.DTOs;

namespace DiamondShopSystem.BLL.Handlers.User.Commands.Get
{
    public class UserGetCommand : IRequest<UserGetResponseDto>
    {
        public Guid UserId { get; }

        public UserGetCommand(Guid userId)
        {
            UserId = userId;
        }
    }
} 