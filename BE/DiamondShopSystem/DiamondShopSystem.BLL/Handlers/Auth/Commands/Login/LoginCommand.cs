using MediatR;
using DiamondShopSystem.BLL.Handlers.Auth.DTOs;

namespace DiamondShopSystem.BLL.Handlers.Auth.Commands.Login
{
    public class LoginCommand : IRequest<LoginResponseDto>
    {
        public LoginRequestDto Request { get; }

        public LoginCommand(LoginRequestDto request)
        {
            Request = request;
        }
    }
} 