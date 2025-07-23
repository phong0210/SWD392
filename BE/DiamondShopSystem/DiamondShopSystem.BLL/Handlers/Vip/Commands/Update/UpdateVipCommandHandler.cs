using MediatR;
using DiamondShopSystem.BLL.Handlers.Vip.DTOs;
using DiamondShopSystem.DAL.Repositories;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;

namespace DiamondShopSystem.BLL.Handlers.Vip.Commands.Update
{
    public class UpdateVipCommandHandler : IRequestHandler<UpdateVipCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateVipCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<bool> Handle(UpdateVipCommand request, CancellationToken cancellationToken)
        {
            var existingVip = await _unitOfWork.VipRepository.GetByIdAsync(request.Id);
            if (existingVip == null)
            {
                return false;
            }

            _mapper.Map(request.Request, existingVip);
            _unitOfWork.VipRepository.Update(existingVip);
            await _unitOfWork.SaveAsync();
            return true;
        }
    }
}
