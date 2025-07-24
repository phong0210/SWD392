using MediatR;
using DiamondShopSystem.BLL.Handlers.Vip.DTOs;
using System.Threading;
using System.Threading.Tasks;
using DiamondShopSystem.BLL.Services.Vip;

namespace DiamondShopSystem.BLL.Handlers.Vip.Commands
{
    public class CreateVipCommandHandler : IRequestHandler<CreateVipCommand, VipDto>
    {
        private readonly IVipService _vipService;

        public CreateVipCommandHandler(IVipService vipService)
        {
            _vipService = vipService;
        }

        public async Task<VipDto> Handle(CreateVipCommand request, CancellationToken cancellationToken)
        {
            return await _vipService.CreateVipAsync(request.Request);
        }
    }
}