using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using DiamondShopSystem.BLL.Services.User;
using DiamondShopSystem.BLL.Handlers.User.DTOs;

namespace DiamondShopSystem.BLL.Handlers.User.Commands.Get
{
    public class UserGetCommandHandler : IRequestHandler<UserGetCommand, UserGetResponseDto>
    {
        private readonly IUserService _userService;

        public UserGetCommandHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<UserGetResponseDto> Handle(UserGetCommand request, CancellationToken cancellationToken)
        {
            return await _userService.GetUserByIdAsync(request.UserId);
        }
    }
} 