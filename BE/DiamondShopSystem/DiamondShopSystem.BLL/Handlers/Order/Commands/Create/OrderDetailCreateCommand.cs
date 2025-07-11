using MediatR;
using DiamondShopSystem.BLL.Handlers.Order.DTOs;

namespace DiamondShopSystem.BLL.Handlers.Order.Commands.Create
{
    public class OrderDetailCreateCommand : IRequest<OrderDetailCreateResponse>
    {
        public OrderDetailCreateDto OrderDetailCreateDto { get; }

        public OrderDetailCreateCommand(OrderDetailCreateDto orderDetailCreateDto)
        {
            OrderDetailCreateDto = orderDetailCreateDto;
        }
    }

    public class OrderDetailCreateResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public OrderDetailDto? Data { get; set; }
    }
} 