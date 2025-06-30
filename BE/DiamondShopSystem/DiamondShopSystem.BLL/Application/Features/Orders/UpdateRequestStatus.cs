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
    public class UpdateRequestStatusCommand : IRequest<OrderDto>
    {
        public Guid OrderId { get; set; }
        public Guid StaffId { get; set; }
        public OrderStatus NewStatus { get; set; }
        public string Notes { get; set; }
    }

    public class UpdateRequestStatusCommandHandler : IRequestHandler<UpdateRequestStatusCommand, OrderDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateRequestStatusCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<OrderDto> Handle(UpdateRequestStatusCommand request, CancellationToken cancellationToken)
        {
            // Validate staff member exists and has appropriate role
            var staff = await _unitOfWork.Users.GetByIdAsync(request.StaffId);
            if (staff == null)
            {
                throw new ArgumentException("Staff member not found");
            }

            // Check if staff has appropriate role
            if (staff.Role?.Name != "SalesStaff" && 
                staff.Role?.Name != "StoreManager" && 
                staff.Role?.Name != "HeadOfficeAdmin")
            {
                throw new UnauthorizedAccessException("Insufficient permissions to update order status");
            }

            // Get the order
            var order = await _unitOfWork.Orders.GetByIdAsync(request.OrderId);
            if (order == null)
            {
                throw new ArgumentException("Order not found");
            }

            // Validate status transition
            if (!IsValidStatusTransition(order.Status, request.NewStatus))
            {
                throw new InvalidOperationException($"Invalid status transition from {order.Status} to {request.NewStatus}");
            }

            // Update order status
            order.Status = request.NewStatus;

            // Update delivery status if applicable
            if (order.Delivery != null)
            {
                switch (request.NewStatus)
                {
                    case OrderStatus.Shipped:
                        order.Delivery.Status = DeliveryStatus.InTransit;
                        order.Delivery.DispatchedDate = DateTime.UtcNow;
                        break;
                    case OrderStatus.Delivered:
                        order.Delivery.Status = DeliveryStatus.Delivered;
                        order.Delivery.DeliveredDate = DateTime.UtcNow;
                        break;
                    case OrderStatus.Canceled:
                        order.Delivery.Status = DeliveryStatus.Failed;
                        break;
                }
            }

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

        private bool IsValidStatusTransition(OrderStatus currentStatus, OrderStatus newStatus)
        {
            return newStatus switch
            {
                OrderStatus.Confirmed => currentStatus == OrderStatus.Pending,
                OrderStatus.Shipped => currentStatus == OrderStatus.Confirmed,
                OrderStatus.Delivered => currentStatus == OrderStatus.Shipped,
                OrderStatus.Canceled => currentStatus == OrderStatus.Pending || currentStatus == OrderStatus.Confirmed,
                _ => false
            };
        }
    }
}