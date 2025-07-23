using MediatR;
using System;

namespace DiamondShopSystem.BLL.Handlers.Vip.Commands
{
    public record DeleteVipCommand(Guid Id) : IRequest<bool>;
}
