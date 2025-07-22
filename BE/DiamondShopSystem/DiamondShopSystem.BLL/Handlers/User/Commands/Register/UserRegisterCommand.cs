using System;
using MediatR;
using DiamondShopSystem.BLL.Handlers.User.DTOs;

namespace DiamondShopSystem.BLL.Handlers.User.Commands.Register
{
    public class UserRegisterCommand : IRequest<UserRegisterResponseDto>
    {
        public UserRegisterDto Dto { get; }

        public UserRegisterCommand(UserRegisterDto dto)
        {
            Dto = dto;
        }
    }
}