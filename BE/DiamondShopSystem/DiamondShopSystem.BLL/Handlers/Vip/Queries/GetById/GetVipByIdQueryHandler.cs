using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using DiamondShopSystem.BLL.Handlers.Vip.DTOs;
using DiamondShopSystem.BLL.Services.Vip;

namespace DiamondShopSystem.BLL.Handlers.Vip.Queries.GetById
{
    public class GetVipByIdQueryHandler : IRequestHandler<GetVipByIdQuery, VipDto?>
    {
        private readonly IVipService _vipService;

        public GetVipByIdQueryHandler(IVipService vipService)
        {
            _vipService = vipService;
        }

        public async Task<VipDto?> Handle(GetVipByIdQuery request, CancellationToken cancellationToken)
        {
            return await _vipService.GetVipByIdAsync(request.Id);
        }
    }
}