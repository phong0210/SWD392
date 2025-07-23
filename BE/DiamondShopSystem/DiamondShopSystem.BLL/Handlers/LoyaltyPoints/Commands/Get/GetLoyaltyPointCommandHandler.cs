
using AutoMapper;
using DiamondShopSystem.BLL.Handlers.LoyaltyPoints.DTOs;
using DiamondShopSystem.BLL.Models;
using DiamondShopSystem.DAL.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace DiamondShopSystem.BLL.Handlers.LoyaltyPoints.Commands.Get
{
    public class GetLoyaltyPointCommandHandler : IRequestHandler<GetLoyaltyPointCommand, LoyaltyPointResponseDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetLoyaltyPointCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<LoyaltyPointResponseDto> Handle(GetLoyaltyPointCommand request, CancellationToken cancellationToken)
        {
            var loyaltyPoint = await _unitOfWork.Repository<DAL.Entities.LoyaltyPoints>().GetByIdAsync(request.Id);
            return _mapper.Map<LoyaltyPointResponseDto>(loyaltyPoint);
        }
    }
}
