using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondShopSystem.BLL.Handlers.Order.Commands.Get
{
    public class OrderDetailGetByOrderCommand : MediatR.IRequest<OrderDetailGetByOrderResponse>
    {
        public Guid OrderId { get; }
        public OrderDetailGetByOrderCommand(Guid orderId)
        {
            OrderId = orderId;
        }
    }

    public class OrderDetailGetByOrderResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public IEnumerable<DiamondShopSystem.BLL.Handlers.Order.DTOs.OrderDetailDto>? Data { get; set; }
    }
}
