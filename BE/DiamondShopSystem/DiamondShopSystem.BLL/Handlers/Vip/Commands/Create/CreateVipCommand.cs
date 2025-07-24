using MediatR;
using DiamondShopSystem.BLL.Handlers.Vip.DTOs;

namespace DiamondShopSystem.BLL.Handlers.Vip.Commands
{
    public record CreateVipCommand(VipCreateRequestDto Request) : IRequest<VipDto>;
}
