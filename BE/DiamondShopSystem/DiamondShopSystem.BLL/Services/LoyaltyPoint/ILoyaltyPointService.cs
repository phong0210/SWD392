using DiamondShopSystem.BLL.Handlers.LoyaltyPoints.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DiamondShopSystem.BLL.Services.LoyaltyPoint
{
    public interface ILoyaltyPointService
    {
        Task<IEnumerable<LoyaltyPointDto>> GetAllLoyaltyPointsAsync();
        Task<LoyaltyPointDto> GetLoyaltyPointByIdAsync(Guid id);
        Task AddLoyaltyPointAsync(LoyaltyPointDto loyaltyPointDto);
        Task UpdateLoyaltyPointAsync(LoyaltyPointDto loyaltyPointDto);
        Task DeleteLoyaltyPointAsync(Guid id);
    }
}