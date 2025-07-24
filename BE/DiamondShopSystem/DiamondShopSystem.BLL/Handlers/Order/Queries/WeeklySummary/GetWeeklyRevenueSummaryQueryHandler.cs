using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DiamondShopSystem.BLL.Handlers.Order.DTOs;
using DiamondShopSystem.BLL.Services.Order;

namespace DiamondShopSystem.BLL.Handlers.Order.Queries.WeeklySummary
{
    public class GetWeeklyRevenueSummaryQueryHandler : IRequestHandler<GetWeeklyRevenueSummaryQuery, List<WeeklyRevenueSummaryDto>>
    {
        private readonly IOrderService _orderService;

        public GetWeeklyRevenueSummaryQueryHandler(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task<List<WeeklyRevenueSummaryDto>> Handle(GetWeeklyRevenueSummaryQuery request, CancellationToken cancellationToken)
        {
            return await _orderService.GetWeeklyRevenueSummaryAsync();
        }
    }
}
