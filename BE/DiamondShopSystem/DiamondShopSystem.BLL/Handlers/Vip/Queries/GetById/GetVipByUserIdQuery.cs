using DiamondShopSystem.BLL.Handlers.Vip.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondShopSystem.BLL.Handlers.Vip.Queries.GetById
{
    public class GetVipByUserIdQuery : IRequest<VipDto>
    {
        public Guid UserId { get; set; }

        public GetVipByUserIdQuery(Guid userId)
        {
            UserId = userId;
        }
    }
}
