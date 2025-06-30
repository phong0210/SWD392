using MediatR;
using DiamondShopSystem.BLL.Application.DTOs;
using DiamondShopSystem.BLL.Application.Interfaces;
using DiamondShopSystem.BLL.Domain.Enums;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DiamondShopSystem.BLL.Application.Features.Deliveries
{
    public class UpdateShipmentStatusCommand : IRequest<bool>
    {
        public UpdateShipmentStatusRequest Request { get; set; }
    }

    public class UpdateShipmentStatusCommandHandler : IRequestHandler<UpdateShipmentStatusCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateShipmentStatusCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(UpdateShipmentStatusCommand request, CancellationToken cancellationToken)
        {
            var delivery = await _unitOfWork.Deliveries.GetByIdAsync(request.Request.DeliveryId);

            if (delivery == null)
            {
                return false; // Or throw a NotFoundException
            }

            if (Enum.TryParse(request.Request.NewStatus, true, out DeliveryStatus newStatus))
            {
                delivery.Status = newStatus;
                if (newStatus == DeliveryStatus.Dispatched)
                {
                    delivery.DispatchedDate = DateTime.UtcNow;
                }
                else if (newStatus == DeliveryStatus.Delivered)
                {
                    delivery.DeliveredDate = DateTime.UtcNow;
                }

                await _unitOfWork.Deliveries.UpdateAsync(delivery);
                await _unitOfWork.CommitAsync();
                return true;
            }
            return false; // Invalid status
        }
    }
}