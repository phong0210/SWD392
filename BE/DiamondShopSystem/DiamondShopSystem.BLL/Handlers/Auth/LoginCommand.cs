using MediatR;
using AutoMapper;

namespace DiamondShopSystem.BLL.Handlers.Auth
{
    public class LoginCommand : IRequest<LoginResponseDto>
    {
        public LoginRequestDto Request { get; set; }
        public LoginCommand(LoginRequestDto request)
        {
            Request = request;
        }
    }
} 
 