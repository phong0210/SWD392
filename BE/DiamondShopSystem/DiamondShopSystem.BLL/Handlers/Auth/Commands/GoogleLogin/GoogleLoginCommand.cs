using MediatR;
using DiamondShopSystem.BLL.Handlers.Auth.DTOs;

namespace DiamondShopSystem.BLL.Handlers.Auth.Commands.GoogleLogin
{
    public class GoogleLoginCommand : IRequest<LoginResponseDto>
    {
        public GoogleLoginCommand(GoogleLoginRequestDto googleLoginRequestDto)
        {
            GoogleLoginRequestDto = googleLoginRequestDto;
        }

        public GoogleLoginRequestDto GoogleLoginRequestDto { get; }
    }
}
