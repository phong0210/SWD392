using MediatR;
using DiamondShopSystem.BLL.Handlers.Vip.DTOs;
using DiamondShopSystem.DAL.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;

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
            var vip = await _unitOfWork.VipRepository.GetByIdAsync(request.Id);
            return _mapper.Map<VipDto>(vip);
        }
    }
}
