using MediatR;
using DiamondShopSystem.BLL.Handlers.LoyaltyPoints.DTOs;

namespace DiamondShopSystem.BLL.Handlers.LoyaltyPoints.Commands.UpdateByUserId
{
    public class UpdateLoyaltyPointByUserIdCommand : IRequest<LoyaltyPointDto>
    {
        public Guid UserId { get; set; }
        public LoyaltyPointUpdateDto Dto { get; set; }
    }
}