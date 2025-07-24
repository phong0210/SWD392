using AutoMapper;
using DiamondShopSystem.BLL.Handlers.Vip.DTOs;
using DiamondShopSystem.BLL.Handlers.Vip.Queries.GetById;
using DiamondShopSystem.DAL.Repositories;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DiamondShopSystem.BLL.Handlers.Vip.Queries.GetByUserId
{
    public class GetVipByUserIdQueryHandler : IRequestHandler<GetVipByUserIdQuery, VipDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetVipByUserIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<VipDto> Handle(GetVipByUserIdQuery request, CancellationToken cancellationToken)
        {
            var vipRepo = _unitOfWork.Repository<DAL.Entities.Vip>();
            var vips = await vipRepo.FindAsync(v => v.UserId == request.UserId);
            var vip = vips.FirstOrDefault();

            return vip == null ? null : _mapper.Map<VipDto>(vip);
        }
    }
}