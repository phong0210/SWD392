using MediatR;
using DiamondShopSystem.BLL.Application.DTOs;
using DiamondShopSystem.BLL.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DiamondShopSystem.BLL.Application.Features.Deliveries
{
    public class GetAssignedDeliveriesQuery : IRequest<IEnumerable<DeliveryDto>>
    {
        public Guid DeliveryStaffId { get; set; }
    }

    public class GetAssignedDeliveriesQueryHandler : IRequestHandler<GetAssignedDeliveriesQuery, IEnumerable<DeliveryDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAssignedDeliveriesQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<DeliveryDto>> Handle(GetAssignedDeliveriesQuery request, CancellationToken cancellationToken)
        {
            var deliveries = (await _unitOfWork.Deliveries.ListAllAsync())
                .Where(d => d.DeliveryStaffId == request.DeliveryStaffId)
                .ToList();

            var deliveryDtos = new List<DeliveryDto>();
            foreach (var delivery in deliveries)
            {
                var order = await _unitOfWork.Orders.GetByIdAsync(delivery.OrderId);
                var customer = await _unitOfWork.Users.GetByIdAsync(order.CustomerId);
                var customerProfile = (await _unitOfWork.CustomerProfiles.ListAllAsync()).FirstOrDefault(cp => cp.UserId == customer.Id);

                deliveryDtos.Add(new DeliveryDto(
                    delivery.Id,
                    delivery.OrderId,
                    order.Status.ToString(),
                    order.ShippingAddress.ToString(),
                    customer.FullName,
                    customer.Phone,
                    delivery.Status.ToString(),
                    delivery.DispatchedDate,
                    delivery.DeliveredDate
                ));
            }

            return deliveryDtos;
        }
    }
}