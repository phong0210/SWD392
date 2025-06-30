using MediatR;
using DiamondShopSystem.BLL.Application.DTOs;
using DiamondShopSystem.BLL.Application.Interfaces;
using DiamondShopSystem.BLL.Domain.Entities;
using DiamondShopSystem.BLL.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DiamondShopSystem.BLL.Application.Features.Orders
{
    public class ConfirmOrderCommand : IRequest<OrderDto>
    {
        public Guid OrderId { get; set; }
        public Guid StaffId { get; set; }
        public string Notes { get; set; }
    }

    public class ConfirmOrderCommandHandler : IRequestHandler<ConfirmOrderCommand, OrderDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public ConfirmOrderCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<OrderDto> Handle(ConfirmOrderCommand request, CancellationToken cancellationToken)
        {
            // Validate staff member exists and has appropriate role
            var staff = await _unitOfWork.Users.GetByIdAsync(request.StaffId);
            if (staff == null)
            {
                throw new ArgumentException("Staff member not found");
            }

            // Check if staff has appropriate role (SalesStaff, StoreManager, or HeadOfficeAdmin)
            if (staff.Role?.Name != "SalesStaff" && 
                staff.Role?.Name != "StoreManager" && 
                staff.Role?.Name != "HeadOfficeAdmin")
            {
                throw new UnauthorizedAccessException("Insufficient permissions to confirm orders");
            }

            // Get the order
            var order = await _unitOfWork.Orders.GetByIdAsync(request.OrderId);
            if (order == null)
            {
                throw new ArgumentException("Order not found");
            }

            // Check if order can be confirmed
            if (order.Status != OrderStatus.Pending)
            {
                throw new InvalidOperationException($"Order cannot be confirmed. Current status: {order.Status}");
            }

            // Update order status
            order.Status = OrderStatus.Confirmed;

            // Create delivery record
            var delivery = new Delivery
            {
                Id = Guid.NewGuid(),
                OrderId = order.Id,
                Status = DeliveryStatus.Pending,
                DispatchedDate = null,
                DeliveredDate = null
            };

            order.Delivery = delivery;

            await _unitOfWork.Orders.UpdateAsync(order);
            await _unitOfWork.CommitAsync();

            // Return updated order using positional constructors
            var orderDetailsDtos = order.OrderDetails?.Select(od => new OrderDetailDto(
                od.ProductId,
                od.Quantity,
                od.PriceAtTimeOfPurchase
            )).ToList() ?? new List<OrderDetailDto>();

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