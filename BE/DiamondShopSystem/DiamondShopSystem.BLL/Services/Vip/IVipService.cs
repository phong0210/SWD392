using System;
using System.Threading.Tasks;
using DiamondShopSystem.BLL.Handlers.Vip.DTOs;

namespace DiamondShopSystem.BLL.Services.Vip
{
    public interface IVipService
    {
        Task<VipDto> RegisterVip(Guid userId, RegisterVipRequest request);
        Task<bool> IsUserVip(Guid userId);
        Task<VipDto> GetUserVipStatus(Guid userId);
    }
}