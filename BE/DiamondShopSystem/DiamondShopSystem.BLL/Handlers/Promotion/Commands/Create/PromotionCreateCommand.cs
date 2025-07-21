using MediatR;
using DiamondShopSystem.BLL.Handlers.Promotion.DTOs;

namespace DiamondShopSystem.BLL.Handlers.Promotion.Commands.Create
{
    public class PromotionCreateCommand : IRequest<PromotionDto>
    {
        public PromotionCreateDto Dto { get; }

        public PromotionCreateCommand(PromotionCreateDto dto)
        {
            Dto = dto;
        }
    }
}