using AutoMapper;
using DiamondShopSystem.BLL.Enums;
using DiamondShopSystem.BLL.Handlers.Order.Commands.Create;
using DiamondShopSystem.BLL.Handlers.Order.Commands.Update;
using DiamondShopSystem.BLL.Handlers.Order.DTOs;
using DiamondShopSystem.BLL.Handlers.Order.Queries.GetAll;
using DiamondShopSystem.BLL.Handlers.Order.Queries.GetById;
using DiamondShopSystem.BLL.Handlers.Order.Queries.GetRelations;
using DiamondShopSystem.BLL.Handlers.Order.Queries.GetRevenueSummary;
using DiamondShopSystem.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DiamondShopSystem.BLL.Services.Order
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OrderService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<CreateOrderResponseDto> CreateOrderAsync(CreateOrderDto createOrderDto)
        {
            // Implementation for creating an order
            throw new NotImplementedException();
        }

        public async Task<UpdateOrderResponseDto> UpdateOrderAsync(Guid id, UpdateOrderDto updateOrderDto)
        {
            var orderRepository = _unitOfWork.Repository<DiamondShopSystem.DAL.Entities.Order>();
            var existingOrder = await orderRepository.GetByIdAsync(id);

            if (existingOrder == null)
            {
                return new UpdateOrderResponseDto { Success = false, Error = "Order not found." };
            }

            // Determine the next status
            OrderStatus currentStatus = (OrderStatus)existingOrder.Status;
            OrderStatus nextStatus = GetNextStatus(currentStatus);
            existingOrder.Status = (int)nextStatus;

            orderRepository.Update(existingOrder);
            await _unitOfWork.SaveChangesAsync();

            return new UpdateOrderResponseDto { Success = true };
        }

        private OrderStatus GetNextStatus(OrderStatus currentStatus)
        {
            return currentStatus switch
            {
                OrderStatus.Pending => OrderStatus.Confirmed,
                OrderStatus.Confirmed => OrderStatus.Processing,
                OrderStatus.Processing => OrderStatus.Shipped,
                OrderStatus.Shipped => OrderStatus.Delivered,
                OrderStatus.Delivered => OrderStatus.Cancelled, // Transition from Delivered to Cancelled
                _ => currentStatus // No change for other statuses (including Cancelled)
            };
        }

        public async Task<bool> DeleteOrderAsync(Guid id)
        {
            // Implementation for deleting an order
            throw new NotImplementedException();
        }

        public async Task<List<OrderResponseDto>> GetAllOrdersAsync()
        {
            // Implementation for getting all orders
            throw new NotImplementedException();
        }

        public async Task<GetOrderByIdResponseDto> GetOrderByIdAsync(Guid id)
        {
            // Implementation for getting an order by ID
            throw new NotImplementedException();
        }

        public async Task<GetOrderRelationsResponseDto> GetOrderRelationsAsync(Guid id)
        {
            // Implementation for getting order relations
            throw new NotImplementedException();
        }

        public async Task<GetRevenueSummaryResponseDto> GetRevenueSummaryAsync()
        {
            // Implementation for getting revenue summary
            throw new NotImplementedException();
        }

        public async Task<List<OrderResponseDto>> GetOrdersByUserIdAsync(Guid userId)
        {
            var orders = await _unitOfWork.OrderRepository.GetOrdersByUserIdAsync(userId);
            return _mapper.Map<List<OrderResponseDto>>(orders);
        }
    }
}