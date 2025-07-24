using MediatR;
using DiamondShopSystem.BLL.Handlers.LoyaltyPoints.DTOs;

namespace DiamondShopSystem.BLL.Handlers.LoyaltyPoints.Queries
{
    public class GetAllLoyaltyPointsQuery : IRequest<IEnumerable<LoyaltyPointDto>>
    {
    }
}