
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DiamondShopSystem.BLL.Handlers.Promotion.DTOs;
using DiamondShopSystem.DAL.Repositories;

namespace DiamondShopSystem.BLL.Handlers.Promotion.Commands.Update
{
    public class UpdatePromotionCommandHandler : IRequestHandler<UpdatePromotionCommand, PromotionDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdatePromotionCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PromotionDto> Handle(UpdatePromotionCommand request, CancellationToken cancellationToken)
        {
            var promotionRepo = _unitOfWork.Repository<DAL.Entities.Promotion>();
            var promotion = await promotionRepo.GetByIdAsync(request.Id);

            if (promotion == null)
            {
                return null; // Or throw an exception
            }

            _mapper.Map(request.Dto, promotion);
            promotionRepo.Update(promotion);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<PromotionDto>(promotion);
        }
    }
}
