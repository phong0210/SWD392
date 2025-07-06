using System;
using MediatR;
using DiamondShopSystem.BLL.Handlers.User.DTOs;

namespace DiamondShopSystem.BLL.Handlers.User.Commands.Create
{
    public class UserCreateCommand : IRequest<UserCreateResponseDto>
    {
        public UserCreateDto Dto { get; }

        public UserCreateCommand(UserCreateDto dto)
        {
            Dto = dto;
        }
    }
} 