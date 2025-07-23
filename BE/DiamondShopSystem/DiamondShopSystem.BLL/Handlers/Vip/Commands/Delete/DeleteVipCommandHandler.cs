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
            var vip = await _unitOfWork.VipRepository.GetByIdAsync(request.Id);
            if (vip == null)
            {
                return false;
            }

            _unitOfWork.VipRepository.Delete(vip);
            await _unitOfWork.SaveAsync();
            return true;
        }
    }
}
