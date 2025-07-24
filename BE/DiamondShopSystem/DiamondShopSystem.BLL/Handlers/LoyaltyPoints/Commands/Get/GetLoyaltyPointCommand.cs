
using DiamondShopSystem.BLL.Handlers.LoyaltyPoints.DTOs;
using MediatR;

namespace DiamondShopSystem.BLL.Handlers.LoyaltyPoints.Commands.Get
{
    public class GetLoyaltyPointCommand : IRequest<LoyaltyPointDto>
    {
        public Guid Id { get; }

        public GetLoyaltyPointCommand(Guid id)
        {
            Id = id;
        }
    }
}
