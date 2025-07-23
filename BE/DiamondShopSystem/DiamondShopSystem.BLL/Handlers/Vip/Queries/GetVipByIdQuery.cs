using MediatR;
using DiamondShopSystem.BLL.Handlers.Vip.DTOs;
using System;

namespace DiamondShopSystem.BLL.Handlers.Vip.Queries
{
    public record GetVipByIdQuery(Guid Id) : IRequest<VipDto>;
}
