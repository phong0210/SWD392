using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DiamondShopSystem.BLL.Handlers.Order.DTOs;
using DiamondShopSystem.BLL.Services.Order;

namespace DiamondShopSystem.BLL.Handlers.Order.Queries.DailySummary
{
    public class GetDailyRevenueSummaryQueryHandler : IRequestHandler<GetDailyRevenueSummaryQuery, List<DailyRevenueSummaryDto>>
    {
        private readonly IOrderService _orderService;

        public GetDailyRevenueSummaryQueryHandler(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task<List<DailyRevenueSummaryDto>> Handle(GetDailyRevenueSummaryQuery request, CancellationToken cancellationToken)
        {
            return await _orderService.GetDailyRevenueSummaryAsync();
        }
    }
}
