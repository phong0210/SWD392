using MediatR;
using AutoMapper;
using DiamondShopSystem.DAL.Entities;
using DiamondShopSystem.DAL.Repositories;
using DiamondShopSystem.BLL.Handlers.LoyaltyPoints.DTOs;

namespace DiamondShopSystem.BLL.Handlers.LoyaltyPoints.Commands.Create
{
    public class LoyaltyPointCreateHandler : IRequestHandler<LoyaltyPointCreateCommand, LoyaltyPointDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public LoyaltyPointCreateHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<LoyaltyPointDto> Handle(LoyaltyPointCreateCommand request, CancellationToken cancellationToken)
        {
            var loyaltyPoint = _mapper.Map<DiamondShopSystem.DAL.Entities.LoyaltyPoints>(request.Dto);
            loyaltyPoint.Id = Guid.NewGuid();
            loyaltyPoint.PointsEarned = 0;
            loyaltyPoint.PointsRedeemed = 0;
            loyaltyPoint.LastUpdated = DateTime.UtcNow;

            await _unitOfWork.Repository<DiamondShopSystem.DAL.Entities.LoyaltyPoints>().AddAsync(loyaltyPoint);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<LoyaltyPointDto>(loyaltyPoint);
        }
    }
}