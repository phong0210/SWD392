using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DiamondShopSystem.BLL.Handlers.Order.DTOs;
using DiamondShopSystem.BLL.Services.Order;

namespace DiamondShopSystem.BLL.Handlers.Order.Queries.GetByUserId
{
    public class GetOrdersByUserIdQueryHandler : IRequestHandler<GetOrdersByUserIdQuery, List<OrderResponseDto>>
    {
        private readonly IOrderService _orderService;

        public GetOrdersByUserIdQueryHandler(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task<List<OrderResponseDto>> Handle(GetOrdersByUserIdQuery request, CancellationToken cancellationToken)
        {
            return await _orderService.GetOrdersByUserIdAsync(request.UserId);
        }
    }
}
