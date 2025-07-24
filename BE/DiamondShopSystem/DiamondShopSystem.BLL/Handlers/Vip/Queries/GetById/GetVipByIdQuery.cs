using MediatR;
using DiamondShopSystem.BLL.Handlers.Vip.DTOs;
using System;

namespace DiamondShopSystem.BLL.Handlers.Vip.Queries.GetById
{
    public record GetVipByIdQuery(Guid Id) : IRequest<VipDto>;
}
