using MediatR;
using DiamondShopSystem.BLL.Handlers.Order.DTOs;

namespace DiamondShopSystem.BLL.Handlers.Order.Commands.Get
{
    public class OrderDetailGetAllCommand : IRequest<OrderDetailGetAllResponse>
    {
    }

    public class OrderDetailGetAllResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public IEnumerable<OrderDetailDto> Data { get; set; } = new List<OrderDetailDto>();
    }
} 