using MediatR;
using System.Collections.Generic;
using DiamondShopSystem.BLL.Handlers.Order.DTOs;

namespace DiamondShopSystem.BLL.Handlers.Order.Queries.WeeklySummary
{
    public class GetWeeklyRevenueSummaryQuery : IRequest<List<WeeklyRevenueSummaryDto>>
    {
    }
}
