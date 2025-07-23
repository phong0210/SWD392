
using AutoMapper;
using MediatR;
using DiamondShopSystem.DAL.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace DiamondShopSystem.BLL.Handlers.LoyaltyPoints.Commands.Update
{
    public class UpdateLoyaltyPointCommandHandler : IRequestHandler<UpdateLoyaltyPointCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateLoyaltyPointCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<int> Handle(UpdateLoyaltyPointCommand request, CancellationToken cancellationToken)
        {
            var loyaltyPoint = await _unitOfWork.Repository<DAL.Entities.LoyaltyPoints>().GetByIdAsync(request.Id);
            if (loyaltyPoint == null) return 0; // Or throw exception

            _mapper.Map(request.LoyaltyPoint, loyaltyPoint);
            _unitOfWork.Repository<DAL.Entities.LoyaltyPoints>().Update(loyaltyPoint);
            return await _unitOfWork.SaveChangesAsync();
        }
    }
}
