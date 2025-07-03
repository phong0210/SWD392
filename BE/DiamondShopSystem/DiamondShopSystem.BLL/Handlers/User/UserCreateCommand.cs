using MediatR;

namespace DiamondShopSystem.BLL.Handlers.User
{
    public class UserCreateCommand : IRequest<UserCreateResponseDto>
    {
        public UserCreateDto Dto { get; set; }
        public UserCreateCommand(UserCreateDto dto)
        {
            Dto = dto;
        }
    }
} 