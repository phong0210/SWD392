using MediatR;
using DiamondShopSystem.BLL.Handlers.LoyaltyPoints.DTOs;

namespace DiamondShopSystem.BLL.Handlers.LoyaltyPoints.Queries
{
    public class GetLoyaltyPointByIdQuery : IRequest<LoyaltyPointDto>
    {
        public Guid LoyaltyPointId { get; set; }
    }
}