
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using DiamondShopSystem.DAL.Repositories;

using DiamondShopSystem.BLL.Handlers.LoyaltyPoints.DTOs;

namespace DiamondShopSystem.BLL.Handlers.LoyaltyPoints.Queries.GetAll
{
    public class GetAllLoyaltyPointsQueryHandler : IRequestHandler<GetAllLoyaltyPointsQuery, List<LoyaltyPointResponseDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllLoyaltyPointsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<LoyaltyPointResponseDto>> Handle(GetAllLoyaltyPointsQuery request, CancellationToken cancellationToken)
        {
            var loyaltyPointsRepository = _unitOfWork.Repository<DiamondShopSystem.DAL.Entities.LoyaltyPoints>();
            var loyaltyPoints = await loyaltyPointsRepository.GetAllAsync();

            return _mapper.Map<List<LoyaltyPointResponseDto>>(loyaltyPoints.ToList());
        }
    }
}
