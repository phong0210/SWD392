using MediatR;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DiamondShopSystem.BLL.Handlers.Promotion.DTOs;
using DiamondShopSystem.DAL.Repositories;

namespace DiamondShopSystem.BLL.Handlers.Promotion.Queries
{
    public class GetPromotionByIdQueryHandler : IRequestHandler<GetPromotionByIdQuery, PromotionDto?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetPromotionByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PromotionDto?> Handle(GetPromotionByIdQuery request, CancellationToken cancellationToken)
        {
            var promotionRepo = _unitOfWork.Repository<DAL.Entities.Promotion>();
            var promotion = await promotionRepo.GetByIdAsync(request.PromotionId);
            return promotion == null ? null : _mapper.Map<PromotionDto>(promotion);
        }
    }
}