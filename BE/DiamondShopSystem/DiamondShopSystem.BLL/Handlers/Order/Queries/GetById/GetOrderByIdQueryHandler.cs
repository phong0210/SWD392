using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using DiamondShopSystem.DAL.Repositories;
using DiamondShopSystem.BLL.Handlers.Order.DTOs;
using System.Linq;

namespace DiamondShopSystem.BLL.Handlers.Order.Queries.GetById
{
    public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, GetOrderByIdResponseDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetOrderByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GetOrderByIdResponseDto> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
        {
            var orderRepository = _unitOfWork.Repository<DiamondShopSystem.DAL.Entities.Order>();
            var order = await orderRepository.GetByIdAsync(request.Id);

            if (order == null)
            {
                return new GetOrderByIdResponseDto { Success = false, Error = "Order not found." };
            }

            // Manually load related entities since IGenericRepository does not support Include
            var orderDetailRepository = _unitOfWork.Repository<DiamondShopSystem.DAL.Entities.OrderDetail>();
            var orderDetails = (await orderDetailRepository.FindAsync(od => od.OrderId == order.Id)).ToList();

            var paymentRepository = _unitOfWork.Repository<DiamondShopSystem.DAL.Entities.Payment>();
            var payments = (await paymentRepository.FindAsync(p => p.OrderId == order.Id)).ToList();

            var deliveryRepository = _unitOfWork.Repository<DiamondShopSystem.DAL.Entities.Delivery>();
            var delivery = (await deliveryRepository.FindAsync(d => d.OrderId == order.Id)).FirstOrDefault();

            // Map to DTOs
            var orderDetailsDto = orderDetails.Select(od => new OrderDetailResponseDto
            {
                Id = od.Id,
                OrderId = od.OrderId,
                UnitPrice = od.UnitPrice,
                Quantity = od.Quantity
            }).ToList();

            var paymentsDto = payments.Select(p => new PaymentResponseDto
            {
                Id = p.Id,
                OrderId = p.OrderId,
                Method = p.Method,
                Date = p.Date,
                Amount = p.Amount,
                Status = p.Status
            }).ToList();

            var deliveryDto = delivery != null ? new DeliveryResponseDto
            {
                Id = delivery.Id,
                OrderId = delivery.OrderId,
                DispatchTime = delivery.DispatchTime,
                DeliveryTime = delivery.DeliveryTime,
                ShippingAddress = delivery.ShippingAddress,
                Status = delivery.Status
            } : null;

            var orderDto = new OrderResponseDto
            {
                Id = order.Id,
                UserId = order.UserId,
                TotalPrice = order.TotalPrice,
                OrderDate = order.OrderDate,
                VipApplied = order.VipApplied,
                Status = order.Status,
                SaleStaff = order.SaleStaff,
                OrderDetails = orderDetailsDto,
                Payments = paymentsDto,
                Delivery = deliveryDto
            };

            return new GetOrderByIdResponseDto { Success = true, Order = orderDto };
        }
    }
}