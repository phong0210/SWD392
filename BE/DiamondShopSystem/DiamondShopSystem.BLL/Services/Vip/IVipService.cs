using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DiamondShopSystem.BLL.Handlers.Vip.DTOs;

namespace DiamondShopSystem.BLL.Services.Vip
{
    public interface IVipService
    {
        Task<VipDto> GetVipByIdAsync(Guid vipId);
        Task<List<VipDto>> GetAllVipsAsync();
        Task<VipDto> CreateVipAsync(VipCreateRequestDto createDto);
        Task<VipDto> UpdateVipAsync(Guid vipId, VipUpdateRequestDto updateDto);
        Task<bool> DeleteVipAsync(Guid vipId);
    }
}