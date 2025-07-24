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
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.EF;


namespace DiamondShopSystem.BLL.Services.Order
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly SaleEmail.ISaleEmailService _saleEmailService;

        public OrderService(IUnitOfWork unitOfWork, IMapper mapper, SaleEmail.ISaleEmailService saleEmailService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _saleEmailService = saleEmailService;
        }

        public async Task<CreateOrderResponseDto> CreateOrderAsync(CreateOrderDto createOrderDto)
        {
            // Implementation for creating an order
            throw new NotImplementedException();
        }

        public async Task<UpdateOrderResponseDto> UpdateOrderAsync(Guid id, UpdateOrderDto updateOrderDto)
        {
            var existingOrder = await _unitOfWork.Repository<DiamondShopSystem.DAL.Entities.Order>()
                                            .GetAllQueryable()
                                            .Include(o => o.User)
                                            .FirstOrDefaultAsync(o => o.Id == id);

            if (existingOrder == null)
            {
                return new UpdateOrderResponseDto { Success = false, Error = "Order not found." };
            }

            // Determine the next status
            OrderStatus currentStatus = (OrderStatus)existingOrder.Status;
            OrderStatus nextStatus = GetNextStatus(currentStatus);
            existingOrder.Status = (int)nextStatus;

            _unitOfWork.Repository<DiamondShopSystem.DAL.Entities.Order>().Update(existingOrder);
            await _unitOfWork.SaveChangesAsync();

            // Send email notification
            var orderResponseDto = _mapper.Map<OrderResponseDto>(existingOrder);
            if (existingOrder.User != null)
            {
                await _saleEmailService.SendOrderUpdateEmailAsync(existingOrder.User.Email, orderResponseDto);
            }

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

        public async Task<List<DailyRevenueSummaryDto>> GetDailyRevenueSummaryAsync()
        {
            var dailyRevenue = await _unitOfWork.Repository<DiamondShopSystem.DAL.Entities.Order>()
                                                .GetAllQueryable()
                                                .GroupBy(o => o.OrderDate.Date)
                                                .Select(g => new DailyRevenueSummaryDto
                                                {
                                                    Date = g.Key,
                                                    TotalRevenue = g.Sum(o => o.TotalPrice)
                                                })
                                                .OrderBy(d => d.Date)
                                                .ToListAsync();
            return dailyRevenue;
        }

        public async Task<List<WeeklyRevenueSummaryDto>> GetWeeklyRevenueSummaryAsync()
        {
            var weeklyRevenue = await _unitOfWork.Repository<DiamondShopSystem.DAL.Entities.Order>()
                                                .GetAllQueryable()
                                                .GroupBy(o => o.OrderDate.AddDays(-(int)o.OrderDate.DayOfWeek).Date)
                                                .Select(g => new WeeklyRevenueSummaryDto
                                                {
                                                    WeekStartDate = g.Key,
                                                    WeekEndDate = g.Key.AddDays(6), // Assuming week starts on Monday
                                                    TotalRevenue = g.Sum(o => o.TotalPrice)
                                                })
                                                .OrderBy(w => w.WeekStartDate)
                                                .ToListAsync();
            return weeklyRevenue;
        }
    }
}