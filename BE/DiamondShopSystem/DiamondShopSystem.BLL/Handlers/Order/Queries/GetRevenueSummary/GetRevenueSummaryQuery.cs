using MediatR;
using DiamondShopSystem.BLL.Handlers.Order.DTOs;

namespace DiamondShopSystem.BLL.Handlers.Order.Queries.GetRevenueSummary
{
    public class GetRevenueSummaryQuery : IRequest<GetRevenueSummaryResponseDto>
    {
        // No parameters needed for now, assuming summary is for all time or predefined period
    }
}