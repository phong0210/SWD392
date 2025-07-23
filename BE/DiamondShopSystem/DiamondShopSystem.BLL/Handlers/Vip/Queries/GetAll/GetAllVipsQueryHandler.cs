using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DiamondShopSystem.BLL.Handlers.Vip.DTOs;
using DiamondShopSystem.BLL.Services.Vip;

namespace DiamondShopSystem.BLL.Handlers.Vip.Queries.GetAll
{
    public class GetAllVipsQueryHandler : IRequestHandler<GetAllVipsQuery, IEnumerable<VipDto>>
    {
        private readonly IVipService _vipService;

        public GetAllVipsQueryHandler(IVipService vipService)
        {
            _vipService = vipService;
        }

        public async Task<IEnumerable<VipDto>> Handle(GetAllVipsQuery request, CancellationToken cancellationToken)
        {
            return await _vipService.GetAllVipsAsync();
        }
    }
}