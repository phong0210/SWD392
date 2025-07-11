using MediatR;
using DiamondShopSystem.BLL.Handlers.Order.Commands.Get;
using DiamondShopSystem.BLL.Handlers.Order.DTOs;
using DiamondShopSystem.BLL.Services.Order;

namespace DiamondShopSystem.BLL.Handlers.Order.Commands.Get
{
    public class OrderDetailGetCommandHandler : IRequestHandler<OrderDetailGetCommand, OrderDetailGetResponse>
    {
        private readonly IOrderDetailService _orderDetailService;

        public OrderDetailGetCommandHandler(IOrderDetailService orderDetailService)
        {
            _orderDetailService = orderDetailService;
        }

        public async Task<OrderDetailGetResponse> Handle(OrderDetailGetCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var orderDetail = await _orderDetailService.GetOrderDetailByIdAsync(request.Id);
                
                if (orderDetail == null)
                {
                    return new OrderDetailGetResponse
                    {
                        Success = false,
                        Message = "Order detail not found"
                    };
                }

                return new OrderDetailGetResponse
                {
                    Success = true,
                    Message = "Order detail retrieved successfully",
                    Data = orderDetail
                };
            }
            catch (Exception ex)
            {
                return new OrderDetailGetResponse
                {
                    Success = false,
                    Message = $"Failed to retrieve order detail: {ex.Message}"
                };
            }
        }
    }
} 