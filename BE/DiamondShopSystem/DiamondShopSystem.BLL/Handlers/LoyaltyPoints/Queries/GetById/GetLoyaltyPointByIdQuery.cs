
using DiamondShopSystem.BLL.Handlers.LoyaltyPoints.DTOs;
using DiamondShopSystem.BLL.Models;
using MediatR;

namespace DiamondShopSystem.BLL.Handlers.LoyaltyPoints.Queries.GetById
{
    public class GetLoyaltyPointByIdQuery : IRequest<LoyaltyPointResponseDto>
    {
        public Guid Id { get; }

        public GetLoyaltyPointByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}
