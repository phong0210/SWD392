using MediatR;
using AutoMapper;
using DiamondShopSystem.DAL.Entities;
using DiamondShopSystem.DAL.Repositories;
using DiamondShopSystem.BLL.Handlers.LoyaltyPoints.DTOs;

namespace DiamondShopSystem.BLL.Handlers.LoyaltyPoints.Commands.Update
{
    public class UpdateLoyaltyPointHandler : IRequestHandler<UpdateLoyaltyPointCommand, LoyaltyPointDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateLoyaltyPointHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<LoyaltyPointDto> Handle(UpdateLoyaltyPointCommand request, CancellationToken cancellationToken)
        {
            var loyaltyPoint = await _unitOfWork.Repository<DiamondShopSystem.DAL.Entities.LoyaltyPoints>().GetByIdAsync(request.Id);
            if (loyaltyPoint == null)
                return null;

            _mapper.Map(request.Dto, loyaltyPoint);
            loyaltyPoint.LastUpdated = DateTime.UtcNow;

            _unitOfWork.Repository<DiamondShopSystem.DAL.Entities.LoyaltyPoints>().Update(loyaltyPoint);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<LoyaltyPointDto>(loyaltyPoint);
        }
    }
}
