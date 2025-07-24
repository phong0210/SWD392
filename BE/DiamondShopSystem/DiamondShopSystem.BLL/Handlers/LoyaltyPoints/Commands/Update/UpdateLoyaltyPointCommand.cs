using MediatR;
using DiamondShopSystem.BLL.Handlers.LoyaltyPoints.DTOs;

namespace DiamondShopSystem.BLL.Handlers.LoyaltyPoints.Commands.Update
{
    public class UpdateLoyaltyPointCommand : IRequest<LoyaltyPointDto>
    {
        public Guid Id { get; set; }
        public LoyaltyPointUpdateDto Dto { get; set; }
    }
}