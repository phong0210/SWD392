using MediatR;
using DiamondShopSystem.BLL.Handlers.Vip.DTOs;
using DiamondShopSystem.DAL.Repositories;
using DiamondShopSystem.DAL.Entities;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;

namespace DiamondShopSystem.BLL.Handlers.Vip.Commands
{
    public class CreateVipCommandHandler : IRequestHandler<CreateVipCommand, VipDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateVipCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<VipDto> Handle(CreateVipCommand request, CancellationToken cancellationToken)
        {
            var vip = _mapper.Map<VipDto>(request.Request);
            var vipRepo = _unitOfWork.Repository<VipDto>();
            await vipRepo.AddAsync(vip);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<VipDto>(vip);
        }
    }
}