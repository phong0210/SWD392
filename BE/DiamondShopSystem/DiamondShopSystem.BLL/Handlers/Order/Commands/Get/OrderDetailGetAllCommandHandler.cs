using MediatR;
using DiamondShopSystem.BLL.Handlers.Order.Commands.Get;
using DiamondShopSystem.BLL.Handlers.Order.DTOs;
using DiamondShopSystem.BLL.Services.Order;

namespace DiamondShopSystem.BLL.Handlers.Order.Commands.Get
{
    public class OrderDetailGetAllCommandHandler : IRequestHandler<OrderDetailGetAllCommand, OrderDetailGetAllResponse>
    {
        private readonly IOrderDetailService _orderDetailService;

        public OrderDetailGetAllCommandHandler(IOrderDetailService orderDetailService)
        {
            _orderDetailService = orderDetailService;
        }

        public async Task<OrderDetailGetAllResponse> Handle(OrderDetailGetAllCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var orderDetails = await _orderDetailService.GetAllOrderDetailsAsync();

                return new OrderDetailGetAllResponse
                {
                    Success = true,
                    Message = "Order details retrieved successfully",
                    Data = orderDetails
                };
            }
            catch (Exception ex)
            {
                return new OrderDetailGetAllResponse
                {
                    Success = false,
                    Message = $"Failed to retrieve order details: {ex.Message}"
                };
            }
        }
    }
} 