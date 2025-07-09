using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DiamondShopSystem.DAL.Repositories;
using DiamondShopSystem.BLL.Handlers.Order.DTOs;

namespace DiamondShopSystem.BLL.Handlers.Order.Queries.GetRevenueSummary
{
    public class GetRevenueSummaryQueryHandler : IRequestHandler<GetRevenueSummaryQuery, GetRevenueSummaryResponseDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetRevenueSummaryQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GetRevenueSummaryResponseDto> Handle(GetRevenueSummaryQuery request, CancellationToken cancellationToken)
        {
            var orderRepository = _unitOfWork.Repository<DiamondShopSystem.DAL.Entities.Order>();
            var orders = await orderRepository.GetAllAsync();

            // Calculate total revenue from all orders
            var totalRevenue = orders.Sum(o => o.TotalPrice);

            return new GetRevenueSummaryResponseDto { Success = true, TotalRevenue = totalRevenue };
        }
    }
}