
using AutoMapper;
using DiamondShopSystem.BLL.Handlers.LoyaltyPoints.DTOs;
using DiamondShopSystem.DAL.Repositories;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DiamondShopSystem.BLL.Handlers.LoyaltyPoints.Queries.GetAll
{
    public class GetAllLoyaltyPointQueryHandler : IRequestHandler<GetAllLoyaltyPointQuery, IEnumerable<LoyaltyPointResponseDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllLoyaltyPointQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<LoyaltyPointResponseDto>> Handle(GetAllLoyaltyPointQuery request, CancellationToken cancellationToken)
        {
            var loyaltyPoints = await _unitOfWork.Repository<DAL.Entities.LoyaltyPoints>().GetAllAsync();
            return _mapper.Map<IEnumerable<LoyaltyPointResponseDto>>(loyaltyPoints);
        }
    }
}
