using DiamondShopSystem.BLL.Handlers.LoyaltyPoints.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondShopSystem.BLL.Handlers.LoyaltyPoints.Queries.GetById
{
    public class GetLoyaltyPointByUserIdQuery : IRequest<LoyaltyPointDto>
    {
        public Guid UserId { get; set; }

        public GetLoyaltyPointByUserIdQuery(Guid userId)
        {
            UserId = userId;
        }
    }
}
