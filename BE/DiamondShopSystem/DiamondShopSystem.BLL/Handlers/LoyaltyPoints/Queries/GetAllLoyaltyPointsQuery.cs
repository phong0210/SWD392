
using MediatR;
using DiamondShopSystem.BLL.Handlers.LoyaltyPoints.DTOs;
using System.Collections.Generic;

namespace DiamondShopSystem.BLL.Handlers.LoyaltyPoints.Queries.GetAll
{
    public class GetAllLoyaltyPointsQuery : IRequest<List<LoyaltyPointResponseDto>>
    {
        // No parameters needed for getting all loyalty points
    }
}
