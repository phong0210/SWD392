using AutoMapper;
using DiamondShopSystem.BLL.Handlers.Promotion.DTOs;
using DiamondShopSystem.BLL.Handlers.Vip.DTOs;
using DiamondShopSystem.DAL.Entities;
using DiamondShopSystem.DAL.Repositories;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DiamondShopSystem.BLL.Handlers.Vip.Queries
{
    public class GetVipByIdQueryHandler : IRequestHandler<GetVipByIdQuery, VipDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetVipByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<VipDto> Handle(GetVipByIdQuery request, CancellationToken cancellationToken)
        {
            var vipRepo = _unitOfWork.Repository<DAL.Entities.Vip>();
            var vip = await vipRepo.GetByIdAsync(request.Id);

            return vip == null ? null : _mapper.Map<VipDto>(vip);
        }
    }
}
