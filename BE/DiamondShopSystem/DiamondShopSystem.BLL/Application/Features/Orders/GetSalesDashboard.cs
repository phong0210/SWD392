using MediatR;
using DiamondShopSystem.BLL.Application.DTOs;
using DiamondShopSystem.BLL.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DiamondShopSystem.BLL.Application.Features.Orders
{
    public class GetSalesDashboardQuery : IRequest<SalesDashboardDto>
    {
    }

    public class GetSalesDashboardQueryHandler : IRequestHandler<GetSalesDashboardQuery, SalesDashboardDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetSalesDashboardQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<SalesDashboardDto> Handle(GetSalesDashboardQuery request, CancellationToken cancellationToken)
        {
            var orders = await _unitOfWork.Orders.ListAllAsync();
            var products = await _unitOfWork.Products.ListAllAsync();
            var categories = await _unitOfWork.Categories.ListAllAsync();

            decimal totalSales = orders.Sum(o => o.TotalAmount);
            int totalOrders = orders.Count();
            int totalProducts = products.Count();

            var salesByCategory = orders
                .SelectMany(o => o.OrderDetails)
                .Join(products, od => od.ProductId, p => p.Id, (od, p) => new { od, p })
                .GroupBy(x => x.p.CategoryId)
                .ToDictionary(
                    g => categories.FirstOrDefault(c => c.Id == g.Key)?.Name ?? "Unknown",
                    g => g.Sum(x => x.od.PriceAtTimeOfPurchase * x.od.Quantity)
                );

            var salesTrend = orders
                .GroupBy(o => o.OrderDate.Date)
                .ToDictionary(
                    g => g.Key,
                    g => g.Sum(o => o.TotalAmount)
                );

            return new SalesDashboardDto(
                totalSales,
                totalOrders,
                totalProducts,
                salesByCategory,
                salesTrend
            );
        }
    }
}