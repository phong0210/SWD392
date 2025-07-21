
using System.Threading.Tasks;
using DiamondShopSystem.BLL.Handlers.Promotion.DTOs;

namespace DiamondShopSystem.BLL.Services.Promotion
{
    public interface IPromotionService
    {
        Task<PromotionDto> GetPromotionByIdAsync(int promotionId);
        Task<PromotionDto> CreatePromotionAsync(PromotionDto promotionDto);
    }
}
