using MediatR;
using DiamondShopSystem.DAL.Entities;
using DiamondShopSystem.DAL.Repositories;

namespace DiamondShopSystem.BLL.Handlers.LoyaltyPoints.Commands.Delete
{
    public class DeleteLoyaltyPointHandler : IRequestHandler<DeleteLoyaltyPointCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteLoyaltyPointHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteLoyaltyPointCommand request, CancellationToken cancellationToken)
        {
            var loyaltyPoint = await _unitOfWork.Repository<DiamondShopSystem.DAL.Entities.LoyaltyPoints>().GetByIdAsync(request.Id);
            if (loyaltyPoint == null)
                return false;

            _unitOfWork.Repository<DiamondShopSystem.DAL.Entities.LoyaltyPoints>().Remove(loyaltyPoint);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}