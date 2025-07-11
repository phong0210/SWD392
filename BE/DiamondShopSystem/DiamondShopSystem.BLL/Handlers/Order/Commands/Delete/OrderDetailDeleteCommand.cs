using MediatR;

namespace DiamondShopSystem.BLL.Handlers.Order.Commands.Delete
{
    public class OrderDetailDeleteCommand : IRequest<OrderDetailDeleteResponse>
    {
        public Guid Id { get; }

        public OrderDetailDeleteCommand(Guid id)
        {
            Id = id;
        }
    }

    public class OrderDetailDeleteResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
    }
} 