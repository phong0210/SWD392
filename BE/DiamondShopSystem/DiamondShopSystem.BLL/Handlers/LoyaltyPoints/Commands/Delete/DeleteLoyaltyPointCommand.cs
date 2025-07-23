
using MediatR;

namespace DiamondShopSystem.BLL.Handlers.LoyaltyPoints.Commands.Delete
{
    public class DeleteLoyaltyPointCommand : IRequest<int>
    {
        public Guid Id { get; }

        public DeleteLoyaltyPointCommand(Guid id)
        {
            Id = id;
        }
    }
}
