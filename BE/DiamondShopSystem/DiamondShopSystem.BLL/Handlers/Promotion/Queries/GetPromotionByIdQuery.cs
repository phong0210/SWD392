
using MediatR;
using DiamondShopSystem.BLL.Handlers.Promotion.DTOs;
using System;

namespace DiamondShopSystem.BLL.Handlers.Promotion.Queries
{
    public class GetPromotionByIdQuery : IRequest<PromotionDto>
    {
        public Guid PromotionId { get; set; }
    }
}
