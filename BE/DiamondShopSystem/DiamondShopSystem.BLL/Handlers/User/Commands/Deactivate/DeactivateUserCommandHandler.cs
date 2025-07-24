using DiamondShopSystem.BLL.Handlers.User.DTOs;
using DiamondShopSystem.BLL.Services.User;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DiamondShopSystem.BLL.Handlers.User.Commands.Deactivate
{
    public class DeactivateUserCommandHandler : IRequestHandler<DeactivateUserCommand, UserUpdateResponseDto>
    {
        private readonly IUserService _userService;

        public DeactivateUserCommandHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<UserUpdateResponseDto> Handle(DeactivateUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                return await _userService.DeactivateUserAsync(request.UserId);
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
