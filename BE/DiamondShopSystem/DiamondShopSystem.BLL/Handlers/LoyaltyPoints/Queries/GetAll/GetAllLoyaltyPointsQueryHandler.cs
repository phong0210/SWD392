using MediatR;
using AutoMapper;
using DiamondShopSystem.DAL.Entities;
using DiamondShopSystem.DAL.Repositories;
using DiamondShopSystem.BLL.Handlers.LoyaltyPoints.DTOs;

namespace DiamondShopSystem.BLL.Handlers.LoyaltyPoints.Queries
{
    public class GetAllLoyaltyPointsHandler : IRequestHandler<GetAllLoyaltyPointsQuery, IEnumerable<LoyaltyPointDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllLoyaltyPointsHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<LoyaltyPointDto>> Handle(GetAllLoyaltyPointsQuery request, CancellationToken cancellationToken)
        {
            var loyaltyPoints = await _unitOfWork.Repository<DiamondShopSystem.DAL.Entities.LoyaltyPoints>().GetAllAsync();
            return _mapper.Map<IEnumerable<LoyaltyPointDto>>(loyaltyPoints);
        }
    }
}