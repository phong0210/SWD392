using System;

namespace DiamondShopSystem.BLL.Handlers.Order.DTOs
{
    public class WeeklyRevenueSummaryDto
    {
        public DateTime WeekStartDate { get; set; }
        public DateTime WeekEndDate { get; set; }
        public double TotalRevenue { get; set; }
    }
}
