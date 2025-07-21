
using System.Threading.Tasks;
using AutoMapper;
using DiamondShopSystem.BLL.Handlers.Promotion.DTOs;
using DiamondShopSystem.DAL.Repositories;
using DiamondShopSystem.DAL.Entities;

namespace DiamondShopSystem.BLL.Services.Promotion
{
    public class PromotionService : IPromotionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PromotionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PromotionDto> GetPromotionByIdAsync(Guid promotionId)
        {
            var promotionRepo = _unitOfWork.Repository<DAL.Entities.Promotion>();
            var promotion = await promotionRepo.GetByIdAsync(promotionId);
            return _mapper.Map<PromotionDto>(promotion);
        }

        public async Task<PromotionDto> CreatePromotionAsync(PromotionDto promotionDto)
        {
            var promotionRepo = _unitOfWork.Repository<DAL.Entities.Promotion>();
            var promotion = _mapper.Map<DAL.Entities.Promotion>(promotionDto);
            await promotionRepo.AddAsync(promotion);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<PromotionDto>(promotion);
        }

        public Task<PromotionDto> GetPromotionByIdAsync(int promotionId)
        {
            throw new NotImplementedException();
        }

    }
}
