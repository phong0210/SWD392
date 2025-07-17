using DiamondShopSystem.BLL.Handlers.Warranty.DTOs;
using MediatR;

namespace DiamondShopSystem.BLL.Handlers.Warranty.Commands.Create
{
    public class WarrantyCreateCommand : IRequest<WarrantyInfoDto>
    {
        public WarrantyCreateDto Dto { get; }
        public WarrantyCreateCommand(WarrantyCreateDto dto)
        {
            Dto = dto;
        }
    }
}