using DiamondShopSystem.BLL.Handlers.Warranty.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DiamondShopSystem.BLL.Services.Warranty
{
    public interface IWarrantyService
    {
        Task<IEnumerable<WarrantyGetResponseDto>> GetAllWarrantiesAsync();
        Task<WarrantyGetResponseDto> GetWarrantyByIdAsync(Guid id);
        Task<WarrantyUpdateResponseDto> UpdateWarrantyAsync(Guid id, WarrantyUpdateDto warranty);
        Task DeleteWarrantyAsync(Guid id);
    }
}