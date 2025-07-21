
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DiamondShopSystem.BLL.Handlers.Promotion.DTOs;
using DiamondShopSystem.DAL.Repositories;

namespace DiamondShopSystem.BLL.Handlers.Promotion.Queries
{
    public class GetAllPromotionsQueryHandler : IRequestHandler<GetAllPromotionsQuery, IEnumerable<PromotionDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllPromotionsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PromotionDto>> Handle(GetAllPromotionsQuery request, CancellationToken cancellationToken)
        {
            var promotionRepo = _unitOfWork.Repository<DAL.Entities.Promotion>();
            var promotions = await promotionRepo.GetAllAsync();
            return _mapper.Map<IEnumerable<PromotionDto>>(promotions);
        }
    }
}
