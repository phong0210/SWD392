using DiamondShopSystem.BLL.Handlers.LoyaltyPoints.DTOs;
using DiamondShopSystem.BLL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DiamondShopSystem.BLL.Services.LoyaltyPoint
{
    public interface ILoyaltyPointService
    {
        Task<IEnumerable<LoyaltyPointResponseDto>> GetAllLoyaltyPointsAsync();
        Task<LoyaltyPointResponseDto> GetLoyaltyPointByIdAsync(Guid id);
        Task AddLoyaltyPointAsync(LoyaltyPointResponseDto loyaltyPointDto);
        Task UpdateLoyaltyPointAsync(LoyaltyPointResponseDto loyaltyPointDto);
        Task DeleteLoyaltyPointAsync(Guid id);
    }
}