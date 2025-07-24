using System;

namespace DiamondShopSystem.BLL.Handlers.Order.DTOs
{
    public class DailyRevenueSummaryDto
    {
        public DateTime Date { get; set; }
        public double TotalRevenue { get; set; }
    }
}
