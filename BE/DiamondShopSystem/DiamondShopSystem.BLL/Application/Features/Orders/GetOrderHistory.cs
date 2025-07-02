using MediatR;
using DiamondShopSystem.BLL.Application.DTOs;
using DiamondShopSystem.BLL.Application.Interfaces;
using DiamondShopSystem.BLL.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DiamondShopSystem.BLL.Application.Features.Orders
{
    public class GetOrderHistoryQuery : IRequest<List<OrderSummaryDto>>
    {
        public Guid CustomerId { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    public class GetOrderHistoryQueryHandler : IRequestHandler<GetOrderHistoryQuery, List<OrderSummaryDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetOrderHistoryQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<OrderSummaryDto>> Handle(GetOrderHistoryQuery request, CancellationToken cancellationToken)
        {
            // Validate customer exists
            var customer = await _unitOfWork.Users.GetByIdAsync(request.CustomerId);
            if (customer == null)
            {
                throw new ArgumentException("Customer not found");
            }

            // Get all orders for the customer
            var allOrders = await _unitOfWork.Orders.ListAllAsync();
            var customerOrders = allOrders
                .Where(o => o.CustomerId == request.CustomerId)
                .OrderByDescending(o => o.OrderDate)
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();

            var orderSummaries = new List<OrderSummaryDto>();

            foreach (var order in customerOrders)
            {
                var orderSummary = new OrderSummaryDto(
                    order.Id,
                    order.OrderDate,
                    order.Status,
                    order.TotalAmount,
                    order.OrderDetails?.Count ?? 0,
                    order.Delivery?.Status
                );

                orderSummaries.Add(orderSummary);
            }

            return orderSummaries;
        }
    }
}