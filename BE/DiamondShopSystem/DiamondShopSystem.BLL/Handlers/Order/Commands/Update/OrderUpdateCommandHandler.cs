using MediatR;
using System.Threading;
using System.Threading.Tasks;
using DiamondShopSystem.BLL.Handlers.Order.DTOs;
using DiamondShopSystem.BLL.Services.Order;

namespace DiamondShopSystem.BLL.Handlers.Order.Commands.Update
{
    public class OrderUpdateCommandHandler : IRequestHandler<OrderUpdateCommand, UpdateOrderResponseDto>
    {
        private readonly IOrderService _orderService;

        public OrderUpdateCommandHandler(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task<UpdateOrderResponseDto> Handle(OrderUpdateCommand request, CancellationToken cancellationToken)
        {
            return await _orderService.UpdateOrderAsync(request.Id, request.Dto);
        }
    }
}
