using DiamondShopSystem.BLL.Handlers.Order.Commands.Create;
using DiamondShopSystem.BLL.Handlers.Order.Commands.Update;
using DiamondShopSystem.BLL.Handlers.Order.DTOs;
using DiamondShopSystem.BLL.Handlers.Order.Queries.GetAll;
using DiamondShopSystem.BLL.Handlers.Order.Queries.GetById;
using DiamondShopSystem.BLL.Handlers.Order.Queries.GetRelations;
using DiamondShopSystem.BLL.Handlers.Order.Queries.GetRevenueSummary;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DiamondShopSystem.BLL.Services.Order
{
    public interface IOrderService
    {
        Task<CreateOrderResponseDto> CreateOrderAsync(CreateOrderDto createOrderDto);
        Task<UpdateOrderResponseDto> UpdateOrderAsync(Guid id, UpdateOrderDto updateOrderDto);
        Task<bool> DeleteOrderAsync(Guid id);
        Task<List<OrderResponseDto>> GetAllOrdersAsync();
        Task<GetOrderByIdResponseDto> GetOrderByIdAsync(Guid id);
        Task<GetOrderRelationsResponseDto> GetOrderRelationsAsync(Guid id);
        Task<GetRevenueSummaryResponseDto> GetRevenueSummaryAsync();
        Task<List<OrderResponseDto>> GetOrdersByUserIdAsync(Guid userId);
    }
}
