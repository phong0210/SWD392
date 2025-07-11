using MediatR;
using DiamondShopSystem.BLL.Handlers.Order.Commands.Update;
using DiamondShopSystem.BLL.Handlers.Order.DTOs;
using DiamondShopSystem.BLL.Services.Order;

namespace DiamondShopSystem.BLL.Handlers.Order.Commands.Update
{
    public class OrderDetailUpdateCommandHandler : IRequestHandler<OrderDetailUpdateCommand, OrderDetailUpdateResponse>
    {
        private readonly IOrderDetailService _orderDetailService;

        public OrderDetailUpdateCommandHandler(IOrderDetailService orderDetailService)
        {
            _orderDetailService = orderDetailService;
        }

        public async Task<OrderDetailUpdateResponse> Handle(OrderDetailUpdateCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var orderDetail = await _orderDetailService.UpdateOrderDetailAsync(request.Id, request.OrderDetailUpdateDto);
                
                return new OrderDetailUpdateResponse
                {
                    Success = true,
                    Message = "Order detail updated successfully",
                    Data = orderDetail
                };
            }
            catch (Exception ex)
            {
                return new OrderDetailUpdateResponse
                {
                    Success = false,
                    Message = $"Failed to update order detail: {ex.Message}"
                };
            }
        }
    }
} 