using MediatR;
using DiamondShopSystem.BLL.Application.DTOs;
using DiamondShopSystem.BLL.Application.Interfaces;
using DiamondShopSystem.BLL.Domain.Entities;
using DiamondShopSystem.BLL.Domain.Enums;
using DiamondShopSystem.BLL.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DiamondShopSystem.BLL.Application.Features.Orders
{
    public class PlaceOrderCommand : IRequest<OrderDto>
    {
        public Guid CustomerId { get; set; }
        public List<OrderItemDto> Items { get; set; }
        public Address ShippingAddress { get; set; }
    }

    public class PlaceOrderCommandHandler : IRequestHandler<PlaceOrderCommand, OrderDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public PlaceOrderCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<OrderDto> Handle(PlaceOrderCommand request, CancellationToken cancellationToken)
        {
            // Validate customer exists
            var customer = await _unitOfWork.Users.GetByIdAsync(request.CustomerId);
            if (customer == null)
            {
                throw new ArgumentException("Customer not found");
            }

            // Validate all products exist and have sufficient stock
            var orderDetails = new List<OrderDetail>();
            decimal totalAmount = 0;

            foreach (var item in request.Items)
            {
                var product = await _unitOfWork.Products.GetByIdAsync(item.ProductId);
                if (product == null)
                {
                    throw new ArgumentException($"Product with ID {item.ProductId} not found");
                }

                if (!product.IsActive)
                {
                    throw new InvalidOperationException($"Product {product.Name} is not available");
                }

                if (product.Quantity < item.Quantity)
                {
                    throw new InvalidOperationException($"Insufficient stock for product {product.Name}");
                }

                // Calculate price at time of purchase
                var priceAtPurchase = product.BasePrice;
                
                // Apply any active promotions
                var activePromotions = product.Promotions?.Where(p => 
                    p.StartDate <= DateTime.UtcNow && 
                    p.EndDate >= DateTime.UtcNow).ToList() ?? new List<Promotion>();
                
                if (activePromotions.Any())
                {
                    priceAtPurchase = product.CalculateFinalPrice(activePromotions);
                }

                var orderDetail = new OrderDetail
                {
                    Id = Guid.NewGuid(),
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    PriceAtTimeOfPurchase = priceAtPurchase
                };

                orderDetails.Add(orderDetail);
                totalAmount += priceAtPurchase * item.Quantity;

                // Update product stock
                product.Quantity -= item.Quantity;
                await _unitOfWork.Products.UpdateAsync(product);
            }

            // Create the order
            var order = new Order
            {
                Id = Guid.NewGuid(),
                CustomerId = request.CustomerId,
                OrderDate = DateTime.UtcNow,
                Status = OrderStatus.Pending,
                TotalAmount = totalAmount,
                ShippingAddress = request.ShippingAddress,
                OrderDetails = orderDetails
            };

            await _unitOfWork.Orders.AddAsync(order);
            await _unitOfWork.CommitAsync();

            // Return order DTO using positional constructors
            var orderDetailsDtos = orderDetails.Select(od => new OrderDetailDto(
                od.ProductId,
                od.Quantity,
                od.PriceAtTimeOfPurchase
            )).ToList();

            return new OrderDto(
                order.Id,
                order.CustomerId,
                order.OrderDate,
                order.Status,
                order.TotalAmount,
                order.ShippingAddress,
                orderDetailsDtos
            );
        }
    }
}