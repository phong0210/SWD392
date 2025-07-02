using MediatR;
using DiamondShopSystem.BLL.Application.Interfaces;
using DiamondShopSystem.BLL.Domain.Enums;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DiamondShopSystem.BLL.Application.Features.Orders
{
    public class HandleOrderFailureCommand : IRequest<bool>
    {
        public Guid OrderId { get; set; }
        public string Reason { get; set; }
    }

    public class HandleOrderFailureCommandHandler : IRequestHandler<HandleOrderFailureCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public HandleOrderFailureCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(HandleOrderFailureCommand request, CancellationToken cancellationToken)
        {
            var order = await _unitOfWork.Orders.GetByIdAsync(request.OrderId);

            if (order == null)
            {
                return false; // Or throw a NotFoundException
            }

            if (order.Status == OrderStatus.Canceled || order.Status == OrderStatus.Delivered)
            {
                throw new Exception("Order cannot be marked as failed if it's already canceled or delivered.");
            }

            order.Status = OrderStatus.Canceled; // Mark as canceled due to failure
            // Optionally, add logic for refund processing here

            await _unitOfWork.Orders.UpdateAsync(order);
            await _unitOfWork.CommitAsync();

            return true;
        }
    }
}