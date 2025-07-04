using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using DiamondShopSystem.BLL.Services;
using DiamondShopSystem.BLL.Handlers.Auth;
using DiamondShopSystem.BLL.Handlers.User;

namespace DiamondShopSystem.BLL.Handlers.Auth
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponseDto>
    {
        private readonly AuthService _authService;
        private readonly IMapper _mapper;

        public LoginCommandHandler(AuthService authService, IMapper mapper)
        {
            _authService = authService;
            _mapper = mapper;
        }

        public async Task<LoginResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var authResult = await _authService.AuthenticateUserAsync(request.Request.Email, request.Request.Password);
            
            if (!authResult.IsSuccess)
            {
                return null;
            }

            var userDto = _mapper.Map<UserDto>(authResult.User);
            return new LoginResponseDto { Token = authResult.Token, User = userDto };
        }
    }
} 