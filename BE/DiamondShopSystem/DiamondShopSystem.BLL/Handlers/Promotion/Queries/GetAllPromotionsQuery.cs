
using MediatR;
using System.Collections.Generic;
using DiamondShopSystem.BLL.Handlers.Promotion.DTOs;

namespace DiamondShopSystem.BLL.Handlers.Promotion.Queries
{
    public class GetAllPromotionsQuery : IRequest<IEnumerable<PromotionDto>>
    {
    }
}
