using MediatR;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DiamondShopSystem.BLL.Handlers.Promotion.DTOs;
using DiamondShopSystem.DAL.Repositories;
using DiamondShopSystem.DAL.Entities;

namespace DiamondShopSystem.BLL.Handlers.Promotion.Commands.Create
{
    public class PromotionCreateCommandHandler : IRequestHandler<PromotionCreateCommand, PromotionDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PromotionCreateCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PromotionDto> Handle(PromotionCreateCommand request, CancellationToken cancellationToken)
        {
            var promotionRepo = _unitOfWork.Repository<DAL.Entities.Promotion>();
            var promotion = _mapper.Map<DAL.Entities.Promotion>(request.Dto);
            await promotionRepo.AddAsync(promotion);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<PromotionDto>(promotion);
        }
    }
}