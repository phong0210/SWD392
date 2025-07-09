using MediatR;
using DiamondShopSystem.BLL.Handlers.Order.DTOs;
using System;

namespace DiamondShopSystem.BLL.Handlers.Order.Commands.Delete
{
    public class DeleteOrderCommand : IRequest<DeleteOrderResponseDto>
    {
        public Guid Id { get; }

        public DeleteOrderCommand(Guid id)
        {
            Id = id;
        }
    }
}
