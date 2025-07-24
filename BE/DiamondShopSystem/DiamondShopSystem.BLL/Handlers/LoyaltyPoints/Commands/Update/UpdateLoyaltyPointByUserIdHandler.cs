using MediatR;
using AutoMapper;
using DiamondShopSystem.DAL.Entities;
using DiamondShopSystem.DAL.Repositories;
using DiamondShopSystem.BLL.Handlers.LoyaltyPoints.DTOs;

namespace DiamondShopSystem.BLL.Handlers.LoyaltyPoints.Commands.UpdateByUserId
{
    public class UpdateLoyaltyPointByUserIdHandler : IRequestHandler<UpdateLoyaltyPointByUserIdCommand, LoyaltyPointDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateLoyaltyPointByUserIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<LoyaltyPointDto> Handle(UpdateLoyaltyPointByUserIdCommand request, CancellationToken cancellationToken)
        {
            // Tìm loyalty point theo UserId
            var loyaltyPointRepository = _unitOfWork.Repository<DiamondShopSystem.DAL.Entities.LoyaltyPoints>();
            var loyaltyPoints = await loyaltyPointRepository
                .FindAsync(lp => lp.UserId == request.UserId);

            var loyaltyPoint = loyaltyPoints.FirstOrDefault();

            if (loyaltyPoint == null)
            {
                return null;
            }

            _mapper.Map(request.Dto, loyaltyPoint);
            loyaltyPoint.LastUpdated = DateTime.UtcNow;

            loyaltyPointRepository.Update(loyaltyPoint);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<LoyaltyPointDto>(loyaltyPoint);
        }
    }
}