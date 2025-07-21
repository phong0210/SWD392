
using MediatR;
using System;

namespace DiamondShopSystem.BLL.Handlers.Promotion.Commands.Delete
{
    public class DeletePromotionCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
    }
}
