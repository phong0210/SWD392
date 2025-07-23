
using DiamondShopSystem.BLL.Handlers.LoyaltyPoints.DTOs;
using MediatR;

namespace DiamondShopSystem.BLL.Handlers.LoyaltyPoints.Commands.Get
{
    public class GetLoyaltyPointCommand : IRequest<LoyaltyPointResponseDto>
    {
        public Guid Id { get; }

        public GetLoyaltyPointCommand(Guid id)
        {
            Id = id;
        }
    }
}
