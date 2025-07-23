
using DiamondShopSystem.BLL.Handlers.Delivery.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondShopSystem.BLL.Services.Delivery
{
    public interface IDeliveryService
    {
        Task<IEnumerable<DeliveryDto>> GetAllDeliveriesAsync();
        Task<DeliveryDto> GetDeliveryByIdAsync(Guid id);
        Task<CreateDeliveryDto> CreateDeliveryAsync(CreateDeliveryDto createDeliveryDto);
        Task UpdateDeliveryAsync(Guid id, UpdateDeliveryDto updateDeliveryDto);
        Task DeleteDeliveryAsync(Guid id);
    }
}
