
using MediatR;
using DiamondShopSystem.DAL.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace DiamondShopSystem.BLL.Handlers.LoyaltyPoints.Commands.Delete
{
    public class DeleteLoyaltyPointCommandHandler : IRequestHandler<DeleteLoyaltyPointCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteLoyaltyPointCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(DeleteLoyaltyPointCommand request, CancellationToken cancellationToken)
        {
            var loyaltyPoint = await _unitOfWork.Repository<DAL.Entities.LoyaltyPoints>().GetByIdAsync(request.Id);
            if (loyaltyPoint == null) return 0; 

            _unitOfWork.Repository<DAL.Entities.LoyaltyPoints>().Remove(loyaltyPoint);
            return await _unitOfWork.SaveChangesAsync();
        }
    }
}
