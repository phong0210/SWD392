using MediatR;
using System.Collections.Generic;
using DiamondShopSystem.BLL.Handlers.Vip.DTOs;

namespace DiamondShopSystem.BLL.Handlers.Vip.Queries.GetAll
{
    public class GetAllVipsQuery : IRequest<IEnumerable<VipDto>> { }
}