using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using DiamondShopSystem.DAL.Repositories;

using DiamondShopSystem.BLL.Handlers.Order.DTOs;

namespace DiamondShopSystem.BLL.Handlers.Order.Commands.Update
{
    public class OrderUpdateCommandHandler : IRequestHandler<OrderUpdateCommand, UpdateOrderResponseDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OrderUpdateCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<UpdateOrderResponseDto> Handle(OrderUpdateCommand request, CancellationToken cancellationToken)
        {
            var orderRepository = _unitOfWork.Repository<DiamondShopSystem.DAL.Entities.Order>();
            var existingOrder = await orderRepository.GetByIdAsync(request.Id);

            if (existingOrder == null)
            {
                return new UpdateOrderResponseDto { Success = false, Error = "Order not found." };
            }

            // Update properties from the DTO
            existingOrder.Status = request.Dto.Status;
            existingOrder.SaleStaff = request.Dto.SaleStaff;
            existingOrder.VipApplied = request.Dto.VipApplied;

            orderRepository.Update(existingOrder);
            await _unitOfWork.SaveChangesAsync();

            return new UpdateOrderResponseDto { Success = true };
        }
    }
}
