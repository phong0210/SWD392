using MediatR;
using DiamondShopSystem.DAL.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DiamondShopSystem.BLL.Handlers.Vip.Commands
{
    public class DeleteVipCommandHandler : IRequestHandler<DeleteVipCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteVipCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteVipCommand request, CancellationToken cancellationToken)
        {
            var vipRepo = _unitOfWork.Repository<DiamondShopSystem.DAL.Entities.Vip>();
            var vip = await vipRepo.GetByIdAsync(request.Id);
            if (vip == null)
            {
                return false;
            }

            vipRepo.Remove(vip);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
