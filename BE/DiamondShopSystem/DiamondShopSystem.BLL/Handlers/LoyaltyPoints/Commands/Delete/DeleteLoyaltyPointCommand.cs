using MediatR;

namespace DiamondShopSystem.BLL.Handlers.LoyaltyPoints.Commands.Delete
{
    public class DeleteLoyaltyPointCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
    }
}