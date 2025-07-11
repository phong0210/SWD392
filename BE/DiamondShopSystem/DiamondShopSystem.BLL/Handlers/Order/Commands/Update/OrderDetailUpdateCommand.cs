using MediatR;
using DiamondShopSystem.BLL.Handlers.Order.DTOs;

namespace DiamondShopSystem.BLL.Handlers.Order.Commands.Update
{
    public class OrderDetailUpdateCommand : IRequest<OrderDetailUpdateResponse>
    {
        public Guid Id { get; }
        public OrderDetailUpdateDto OrderDetailUpdateDto { get; }

        public OrderDetailUpdateCommand(Guid id, OrderDetailUpdateDto orderDetailUpdateDto)
        {
            Id = id;
            OrderDetailUpdateDto = orderDetailUpdateDto;
        }
    }

    public class OrderDetailUpdateResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public OrderDetailDto? Data { get; set; }
    }
} 