using MediatR;
using DiamondShopSystem.BLL.Services.User;
using System;
using System.Threading;
using System.Threading.Tasks;
using DiamondShopSystem.BLL.Handlers.User.DTOs;

namespace DiamondShopSystem.BLL.Handlers.User.Commands.RemoveStaffRole
{
    public class RemoveStaffRoleCommandHandler : IRequestHandler<RemoveStaffRoleCommand, UserUpdateResponseDto>
    {
        private readonly IUserService _userService;

        public RemoveStaffRoleCommandHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<UserUpdateResponseDto> Handle(RemoveStaffRoleCommand request, CancellationToken cancellationToken)
        {
            try
            {
                return await _userService.RemoveStaffRoleAsync(request.UserId);
            }
            catch (Exception ex)
            {
                return new UserUpdateResponseDto
                {
                    Success = false,
                    Error = ex.Message
                };
            }
        }
    }
}
