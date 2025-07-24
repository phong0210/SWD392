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
using System.Linq;
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
            // Validate input
            if (createOrderDto.CustomerId == Guid.Empty)
            {
                return new CreateOrderResponseDto { Success = false, Error = "Invalid customer ID." };
            }
            if (createOrderDto.OrderItems == null || !createOrderDto.OrderItems.Any())
            {
                return new CreateOrderResponseDto { Success = false, Error = "Order must contain at least one item." };
            }
            if (createOrderDto.OrderItems.Any(item => item.ProductId == Guid.Empty || item.Quantity <= 0))
            {
                return new CreateOrderResponseDto { Success = false, Error = "Invalid product ID or quantity in order items." };
            }

            // Map DTO to Order entity
            var order = _mapper.Map<DiamondShopSystem.DAL.Entities.Order>(createOrderDto);
            order.Id = Guid.NewGuid();
            order.OrderDate = DateTime.UtcNow;
            order.Status = (int)OrderStatus.Pending;
            order.UserId = createOrderDto.CustomerId; // Map CustomerId to UserId

            // Map OrderItems to OrderDetail entities
            var orderDetails = new List<DiamondShopSystem.DAL.Entities.OrderDetail>();
            foreach (var item in createOrderDto.OrderItems)
            {
                // Validate product existence
                var product = await _unitOfWork.Repository<DiamondShopSystem.DAL.Entities.Product>()
                                              .GetAllQueryable()
                                              .FirstOrDefaultAsync(p => p.Id == item.ProductId);
                if (product == null)
                {
                    return new CreateOrderResponseDto { Success = false, Error = $"Product not found for ProductId {item.ProductId}." };
                }

                orderDetails.Add(new DiamondShopSystem.DAL.Entities.OrderDetail
                {
                    Id = Guid.NewGuid(),
                    OrderId = order.Id,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice // Use UnitPrice from DTO
                });
            }

            // Calculate TotalPrice
            order.TotalPrice = orderDetails.Sum(od => od.Quantity * od.UnitPrice);

            // Add order and order details to repository
            await _unitOfWork.Repository<DiamondShopSystem.DAL.Entities.Order>().AddAsync(order);
            foreach (var orderDetail in orderDetails)
            {
                await _unitOfWork.Repository<DiamondShopSystem.DAL.Entities.OrderDetail>().AddAsync(orderDetail);
            }

            // Save changes
            await _unitOfWork.SaveChangesAsync();

            // Map to response DTO
            var response = _mapper.Map<CreateOrderResponseDto>(order);
            response.Success = true;
            return response;
        }

        public async Task<UpdateOrderResponseDto> UpdateOrderAsync(Guid id, UpdateOrderDto updateOrderDto)
        {
            var existingOrder = await _unitOfWork.Repository<DiamondShopSystem.DAL.Entities.Order>()
                                            .GetAllQueryable()
                                            .Include(o => o.User)
                                            .Include(o => o.OrderDetails)
                                            .ThenInclude(od => od.Product)
                                            .FirstOrDefaultAsync(o => o.Id == id);

            if (existingOrder == null)
            {
                return new UpdateOrderResponseDto { Success = false, Error = "Order not found." };
            }

            // Determine the next status
            OrderStatus currentStatus = (OrderStatus)existingOrder.Status;
            OrderStatus nextStatus = GetNextStatus(currentStatus);

            // Check if transitioning from Pending to Confirmed
            if (currentStatus == OrderStatus.Pending && nextStatus == OrderStatus.Confirmed)
            {
                // Check and update inventory
                foreach (var orderDetail in existingOrder.OrderDetails)
                {
                    var product = orderDetail.Product;
                    if (product == null)
                    {
                        return new UpdateOrderResponseDto { Success = false, Error = $"Product not found for OrderDetail {orderDetail.Id}." };
                    }

                    if (product.StockQuantity < orderDetail.Quantity)
                    {
                        return new UpdateOrderResponseDto { Success = false, Error = $"Insufficient stock for product {product.Name}. Available: {product.StockQuantity}, Requested: {orderDetail.Quantity}." };
                    }

                    // Deduct the stock quantity
                    product.StockQuantity -= orderDetail.Quantity;
                    _unitOfWork.Repository<DiamondShopSystem.DAL.Entities.Product>().Update(product);
                }
            }

            // Update the order status
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
                OrderStatus.Confirmed => OrderStatus.Delivering,
                OrderStatus.Delivering => OrderStatus.Delivered,
                OrderStatus.Delivered => OrderStatus.Completed,
                OrderStatus.Completed => OrderStatus.Confirm,
                OrderStatus.Confirm => OrderStatus.Cancelled,
                OrderStatus.Cancelled => OrderStatus.Cancelled,
                _ => currentStatus
            };
        }

        public async Task<bool> DeleteOrderAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<OrderResponseDto>> GetAllOrdersAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<GetOrderByIdResponseDto> GetOrderByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<GetOrderRelationsResponseDto> GetOrderRelationsAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<GetRevenueSummaryResponseDto> GetRevenueSummaryAsync()
        {
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
                                                    WeekEndDate = g.Key.AddDays(6),
                                                    TotalRevenue = g.Sum(o => o.TotalPrice)
                                                })
                                                .OrderBy(w => w.WeekStartDate)
                                                .ToListAsync();
            return weeklyRevenue;
        }
    }
}