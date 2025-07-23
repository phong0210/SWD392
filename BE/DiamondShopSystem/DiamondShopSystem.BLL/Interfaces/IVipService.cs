using System;
using System.Threading.Tasks;
using DiamondShopSystem.BLL.Handlers.Vip.DTOs;

namespace DiamondShopSystem.BLL.Interfaces
{
    public interface IVipService
    {
        Task<VipGetResponseDto> GetVipByIdAsync(Guid vipId);
        Task<VipListResponseDto> GetAllVipsAsync();
        Task<VipCreateResponseDto> CreateVipAsync(VipCreateRequestDto createDto);
        Task<VipUpdateResponseDto> UpdateVipAsync(Guid vipId, VipUpdateRequestDto updateDto);
        Task<VipDeleteResponseDto> DeleteVipAsync(Guid vipId);
    }
}