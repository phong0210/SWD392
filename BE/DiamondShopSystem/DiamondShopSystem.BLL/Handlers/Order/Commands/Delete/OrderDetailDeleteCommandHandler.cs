using MediatR;
using DiamondShopSystem.BLL.Handlers.Order.Commands.Delete;
using DiamondShopSystem.BLL.Services.Order;

namespace DiamondShopSystem.BLL.Handlers.Order.Commands.Delete
{
    public class OrderDetailDeleteCommandHandler : IRequestHandler<OrderDetailDeleteCommand, OrderDetailDeleteResponse>
    {
        private readonly IOrderDetailService _orderDetailService;

        public OrderDetailDeleteCommandHandler(IOrderDetailService orderDetailService)
        {
            _orderDetailService = orderDetailService;
        }

        public async Task<OrderDetailDeleteResponse> Handle(OrderDetailDeleteCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _orderDetailService.DeleteOrderDetailAsync(request.Id);
                
                if (!result)
                {
                    return new OrderDetailDeleteResponse
                    {
                        Success = false,
                        Message = "Order detail not found"
                    };
                }

                return new OrderDetailDeleteResponse
                {
                    Success = true,
                    Message = "Order detail deleted successfully"
                };
            }
            catch (Exception ex)
            {
                return new OrderDetailDeleteResponse
                {
                    Success = false,
                    Message = $"Failed to delete order detail: {ex.Message}"
                };
            }
        }
    }
} 