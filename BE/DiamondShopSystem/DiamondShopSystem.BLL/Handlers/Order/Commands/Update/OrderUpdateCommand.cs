using MediatR;
using DiamondShopSystem.BLL.Handlers.Order.DTOs;
using System;

namespace DiamondShopSystem.BLL.Handlers.Order.Commands.Update
{
    public class OrderUpdateCommand : IRequest<UpdateOrderResponseDto>
    {
        public Guid Id { get; }
        public UpdateOrderDto Dto { get; }

        public OrderUpdateCommand(Guid id, UpdateOrderDto dto)
        {
            Id = id;
            Dto = dto;
        }
    }
}
