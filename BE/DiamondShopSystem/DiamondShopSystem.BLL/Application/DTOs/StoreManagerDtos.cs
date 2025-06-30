using System;
using System.Collections.Generic;

namespace DiamondShopSystem.BLL.Application.DTOs
{
    public record UpdateInventoryRequest(
        Guid ProductId,
        int QuantityChange // Can be positive for increase, negative for decrease
    );

    public record SalesDashboardDto(
        decimal TotalSales,
        int TotalOrders,
        int TotalProducts,
        Dictionary<string, decimal> SalesByCategory,
        Dictionary<DateTime, decimal> SalesTrend
    );
}