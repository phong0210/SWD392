
using MediatR;
using System;
using DiamondShopSystem.BLL.Handlers.Promotion.DTOs;

namespace DiamondShopSystem.BLL.Handlers.Promotion.Commands.Update
{
    public class UpdatePromotionCommand : IRequest<PromotionDto>
    {
        public Guid Id { get; set; }
        public PromotionUpdateDto Dto { get; set; }
    }
}
