
using AutoMapper;
using MediatR;
using DiamondShopSystem.DAL.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace DiamondShopSystem.BLL.Handlers.LoyaltyPoints.Commands.Create
{
    public class LoyaltyPointCreateCommandHandler : IRequestHandler<LoyaltyPointCreateCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public LoyaltyPointCreateCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<int> Handle(LoyaltyPointCreateCommand request, CancellationToken cancellationToken)
        {
            var loyaltyPoint = _mapper.Map<DAL.Entities.LoyaltyPoints>(request.LoyaltyPoint);
            await _unitOfWork.Repository<DAL.Entities.LoyaltyPoints>().AddAsync(loyaltyPoint);
            return await _unitOfWork.SaveChangesAsync();
        }
    }
}
