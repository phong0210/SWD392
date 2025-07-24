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
                var orderDetails = await _orderDetailService.GetOrderDetailsByOrderIdAsync(request.OrderId);

                return new OrderDetailGetByOrderResponse
                {
                    Success = true,
                    Message = "Order details retrieved successfully",
                    Data = orderDetails
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
}