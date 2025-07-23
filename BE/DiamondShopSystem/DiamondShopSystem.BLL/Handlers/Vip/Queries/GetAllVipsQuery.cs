using MediatR;
using DiamondShopSystem.BLL.Handlers.Vip.DTOs;
using System.Collections.Generic;

namespace DiamondShopSystem.BLL.Handlers.Vip.Queries
{
    public record GetAllVipsQuery : IRequest<IEnumerable<VipDto>>;
}
