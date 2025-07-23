
using DiamondShopSystem.BLL.Handlers.LoyaltyPoints.DTOs;
using DiamondShopSystem.BLL.Models;
using MediatR;

namespace DiamondShopSystem.BLL.Handlers.LoyaltyPoints.Commands.Create
{
    public class LoyaltyPointCreateCommand : IRequest<int>
    {
        public LoyaltyPointResponseDto LoyaltyPoint { get; }

        public LoyaltyPointCreateCommand(LoyaltyPointResponseDto loyaltyPoint)
        {
            LoyaltyPoint = loyaltyPoint;
        }
    }
}
