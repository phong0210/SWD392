using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DiamondShopSystem.BLL.Handlers.Order.DTOs;

namespace DiamondShopSystem.BLL.Services.Order
{
    public interface IOrderDetailService
    {
        Task<OrderDetailDto> CreateOrderDetailAsync(OrderDetailCreateDto dto);
        Task<OrderDetailDto?> GetOrderDetailByIdAsync(Guid id);
        Task<IEnumerable<OrderDetailDto>> GetAllOrderDetailsAsync();
        Task<IEnumerable<OrderDetailDto>> GetOrderDetailsByOrderIdAsync(Guid orderId);
        Task<OrderDetailDto> UpdateOrderDetailAsync(Guid id, OrderDetailUpdateDto dto);
        Task<bool> DeleteOrderDetailAsync(Guid id);
    }
} 