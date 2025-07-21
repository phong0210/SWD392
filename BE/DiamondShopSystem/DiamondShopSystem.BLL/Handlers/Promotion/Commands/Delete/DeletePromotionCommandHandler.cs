
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using DiamondShopSystem.DAL.Repositories;

namespace DiamondShopSystem.BLL.Handlers.Promotion.Commands.Delete
{
    public class DeletePromotionCommandHandler : IRequestHandler<DeletePromotionCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeletePromotionCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeletePromotionCommand request, CancellationToken cancellationToken)
        {
            var promotionRepo = _unitOfWork.Repository<DAL.Entities.Promotion>();
            var promotion = await promotionRepo.GetByIdAsync(request.Id);

            if (promotion == null)
            {
                return false;
            }

            promotionRepo.Remove(promotion);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
