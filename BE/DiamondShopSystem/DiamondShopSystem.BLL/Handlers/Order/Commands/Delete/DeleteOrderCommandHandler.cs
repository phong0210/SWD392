using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using DiamondShopSystem.DAL.Repositories;

using DiamondShopSystem.BLL.Handlers.Order.DTOs;

namespace DiamondShopSystem.BLL.Handlers.Order.Commands.Delete
{
    public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand, DeleteOrderResponseDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteOrderCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<DeleteOrderResponseDto> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            var orderRepository = _unitOfWork.Repository<DiamondShopSystem.DAL.Entities.Order>();
            var existingOrder = await orderRepository.GetByIdAsync(request.Id);

            if (existingOrder == null)
            {
                return new DeleteOrderResponseDto { Success = false, Error = "Order not found." };
            }

            var orderDetailRepository = _unitOfWork.Repository<DiamondShopSystem.DAL.Entities.OrderDetail>();
            var productRepository = _unitOfWork.Repository<DiamondShopSystem.DAL.Entities.Product>();
            var orderDetails = await orderDetailRepository.FindAsync(od => od.OrderId == request.Id);

            foreach (var orderDetail in orderDetails)
            {
                var productsToUpdate = await productRepository.FindAsync(p => p.OrderDetailId == orderDetail.Id);
                foreach (var product in productsToUpdate)
                {
                    product.OrderDetailId = null;
                    productRepository.Update(product);
                }
                orderDetailRepository.Remove(orderDetail);
            }

            var paymentRepository = _unitOfWork.Repository<DiamondShopSystem.DAL.Entities.Payment>();
            var payments = await paymentRepository.FindAsync(p => p.OrderId == request.Id);
            foreach (var payment in payments)
            {
                paymentRepository.Remove(payment);
            }

            var deliveryRepository = _unitOfWork.Repository<DiamondShopSystem.DAL.Entities.Delivery>();
            var deliveries = await deliveryRepository.FindAsync(d => d.OrderId == request.Id);
            foreach (var delivery in deliveries)
            {
                deliveryRepository.Remove(delivery);
            }

            orderRepository.Remove(existingOrder);
            await _unitOfWork.SaveChangesAsync();

            return new DeleteOrderResponseDto { Success = true };
        }
    }
}
