using MediatR;
using DiamondShopSystem.BLL.Handlers.Order.DTOs;

namespace DiamondShopSystem.BLL.Handlers.Order.Commands.Get
{
    public class OrderDetailGetCommand : IRequest<OrderDetailGetResponse>
    {
        public Guid Id { get; }

        public OrderDetailGetCommand(Guid id)
        {
            Id = id;
        }
    }

    public class OrderDetailGetResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public OrderDetailDto? Data { get; set; }
    }
} 