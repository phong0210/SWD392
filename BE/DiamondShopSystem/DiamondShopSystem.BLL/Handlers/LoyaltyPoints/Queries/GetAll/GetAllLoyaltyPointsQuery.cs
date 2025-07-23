
using DiamondShopSystem.BLL.Handlers.LoyaltyPoints.DTOs;
using MediatR;
using System.Collections.Generic;

namespace DiamondShopSystem.BLL.Handlers.LoyaltyPoints.Queries.GetAll
{
    public class GetAllLoyaltyPointQuery : IRequest<IEnumerable<LoyaltyPointResponseDto>>
    {
    }
}
