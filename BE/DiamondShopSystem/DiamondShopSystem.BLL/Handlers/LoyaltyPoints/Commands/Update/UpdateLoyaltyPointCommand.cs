
using DiamondShopSystem.BLL.Handlers.LoyaltyPoints.DTOs;
using MediatR;

namespace DiamondShopSystem.BLL.Handlers.LoyaltyPoints.Commands.Update
{
    public class UpdateLoyaltyPointCommand : IRequest<int>
    {
        public Guid Id { get; }
        public LoyaltyPointResponseDto LoyaltyPoint { get; }

        public UpdateLoyaltyPointCommand(Guid id, LoyaltyPointResponseDto loyaltyPoint)
        {
            Id = id;
            LoyaltyPoint = loyaltyPoint;
        }
    }
}
