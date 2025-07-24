using MediatR;
using DiamondShopSystem.BLL.Handlers.LoyaltyPoints.DTOs;

namespace DiamondShopSystem.BLL.Handlers.LoyaltyPoints.Commands.Create
{
    public class LoyaltyPointCreateCommand : IRequest<LoyaltyPointDto>
    {
        public LoyaltyPointCreateDto Dto { get; set; }

        public LoyaltyPointCreateCommand(LoyaltyPointCreateDto dto)
        {
            Dto = dto;
        }
    }
}