using MediatR;
using AutoMapper;
using DiamondShopSystem.DAL.Entities;
using DiamondShopSystem.DAL.Repositories;
using DiamondShopSystem.BLL.Handlers.LoyaltyPoints.DTOs;

namespace DiamondShopSystem.BLL.Handlers.LoyaltyPoints.Queries
{
    public class GetLoyaltyPointByIdHandler : IRequestHandler<GetLoyaltyPointByIdQuery, LoyaltyPointDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetLoyaltyPointByIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<LoyaltyPointDto> Handle(GetLoyaltyPointByIdQuery request, CancellationToken cancellationToken)
        {
            var loyaltyPoint = await _unitOfWork.Repository<DiamondShopSystem.DAL.Entities.LoyaltyPoints>().GetByIdAsync(request.LoyaltyPointId);
            return _mapper.Map<LoyaltyPointDto>(loyaltyPoint);
        }
    }
}