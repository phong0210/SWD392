using AutoMapper;
using DiamondShopSystem.BLL.Handlers.LoyaltyPoints.DTOs;
using DiamondShopSystem.BLL.Handlers.LoyaltyPoints.Queries.GetById;
using DiamondShopSystem.DAL.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondShopSystem.BLL.Handlers.LoyaltyPoints.Queries.GetById
{
    public class GetLoyaltyPointByUserIdQueryHandler : IRequestHandler<GetLoyaltyPointByUserIdQuery, LoyaltyPointDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetLoyaltyPointByUserIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<LoyaltyPointDto> Handle(GetLoyaltyPointByUserIdQuery request, CancellationToken cancellationToken)
        {
            var vipRepo = _unitOfWork.Repository<DAL.Entities.LoyaltyPoints>();
            var vips = await vipRepo.FindAsync(v => v.UserId == request.UserId);
            var vip = vips.FirstOrDefault();

            return vip == null ? null : _mapper.Map<LoyaltyPointDto>(vip);
        }
    }
}
