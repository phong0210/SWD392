using MediatR;
using DiamondShopSystem.BLL.Application.DTOs;
using DiamondShopSystem.BLL.Application.Interfaces;
using DiamondShopSystem.BLL.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DiamondShopSystem.BLL.Application.Features.Orders
{
    public class GetOrderStatusQuery : IRequest<OrderStatusDto>
    {
        public Guid OrderId { get; set; }
        public Guid CustomerId { get; set; } // For authorization
    }

    public class GetOrderStatusQueryHandler : IRequestHandler<GetOrderStatusQuery, OrderStatusDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetOrderStatusQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<OrderStatusDto> Handle(GetOrderStatusQuery request, CancellationToken cancellationToken)
        {
            var order = await _unitOfWork.Orders.GetByIdAsync(request.OrderId);
            if (order == null)
            {
                throw new ArgumentException("Order not found");
            }

            // Check if the customer is authorized to view this order
            if (order.CustomerId != request.CustomerId)
            {
                throw new UnauthorizedAccessException("You are not authorized to view this order");
            }

            return new OrderStatusDto(
                order.Id,
                order.Status,
                order.OrderDate,
                order.TotalAmount,
                order.ShippingAddress,
                order.Delivery?.Status,
                order.Delivery?.DispatchedDate,
                order.Delivery?.DeliveredDate
            );
        }
    }
}