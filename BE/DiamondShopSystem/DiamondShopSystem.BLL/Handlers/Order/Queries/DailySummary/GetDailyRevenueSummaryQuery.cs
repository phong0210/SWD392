using MediatR;
using System.Collections.Generic;
using DiamondShopSystem.BLL.Handlers.Order.DTOs;

namespace DiamondShopSystem.BLL.Handlers.Order.Queries.DailySummary
{
    public class GetDailyRevenueSummaryQuery : IRequest<List<DailyRevenueSummaryDto>>
    {
    }
}
