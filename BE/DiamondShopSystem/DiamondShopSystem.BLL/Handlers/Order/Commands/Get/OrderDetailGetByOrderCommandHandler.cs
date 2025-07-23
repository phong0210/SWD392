using MediatR;
using DiamondShopSystem.BLL.Handlers.Order.Commands.Get;
using DiamondShopSystem.BLL.Handlers.Order.DTOs;
using DiamondShopSystem.BLL.Services.Order;

namespace DiamondShopSystem.BLL.Handlers.Order.Commands.Get
{
    public class OrderDetailGetByOrderCommandHandler : IRequestHandler<OrderDetailGetByOrderCommand, OrderDetailGetByOrderResponse>
    {
        private readonly IOrderDetailService _orderDetailService;

        public OrderDetailGetByOrderCommandHandler(IOrderDetailService orderDetailService)
        {
            _orderDetailService = orderDetailService;
        }

        public async Task<OrderDetailGetByOrderResponse> Handle(OrderDetailGetByOrderCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var orderDetails = await _orderDetailService.GetOrderDetailByIdAsync(request.OrderId);

                var orderDetailsCollection = orderDetails is IEnumerable<OrderDetailDto>
                    ? (IEnumerable<OrderDetailDto>)orderDetails
                    : new List<OrderDetailDto> { orderDetails };

                return new OrderDetailGetByOrderResponse
                {
                    Success = true,
                    Message = "Order details retrieved successfully",
                    Data = orderDetailsCollection
                };
            }
            catch (Exception ex)
            {
                return new OrderDetailGetByOrderResponse
                {
                    Success = false,
                    Message = $"Failed to retrieve order details: {ex.Message}"
                };
            }
        }
    }

    // Add missing command and response classes
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