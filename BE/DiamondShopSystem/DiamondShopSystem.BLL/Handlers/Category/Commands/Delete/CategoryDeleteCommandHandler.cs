using System.Threading;
using System.Threading.Tasks;
using DiamondShopSystem.DAL.Entities;
using DiamondShopSystem.DAL.Repositories;
using MediatR;

namespace DiamondShopSystem.BLL.Handlers.Category.Commands.Delete
{
    public class CategoryDeleteCommandHandler : IRequestHandler<CategoryDeleteCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryDeleteCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(CategoryDeleteCommand request, CancellationToken cancellationToken)
        {
            var repo = _unitOfWork.Repository<DiamondShopSystem.DAL.Entities.Category>();
            var category = await repo.GetByIdAsync(request.CategoryId);
            if (category == null)
                return false;

            repo.Remove(category);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}   