using MediatR;
using DiamondShopSystem.BLL.Handlers.Vip.DTOs;
using System;

namespace DiamondShopSystem.BLL.Handlers.Vip.Commands
{
    public record UpdateVipCommand(Guid Id, VipUpdateRequestDto Request) : IRequest<bool>;
}
