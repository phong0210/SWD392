using MediatR;
using System;
using DiamondShopSystem.BLL.Handlers.Vip.DTOs;

namespace DiamondShopSystem.BLL.Handlers.Vip.Queries.GetById
{
    public class GetVipByIdQuery : IRequest<VipDto?>
    {
        public Guid Id { get; set; }
    }
}