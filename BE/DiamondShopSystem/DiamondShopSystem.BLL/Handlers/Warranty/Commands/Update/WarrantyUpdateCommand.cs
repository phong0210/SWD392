using DiamondShopSystem.BLL.Handlers.Warranty.DTOs;
using MediatR;
using System;

namespace DiamondShopSystem.BLL.Handlers.Warranty.Commands.Update
{
    public class WarrantyUpdateCommand : IRequest<WarrantyUpdateResponseDto>
    {
        public Guid WarrantyId { get; }
        public WarrantyUpdateDto Dto { get; }
        public WarrantyUpdateCommand(Guid warrantyId, WarrantyUpdateDto dto)
        {
            WarrantyId = warrantyId;
            Dto = dto;
        }
    }
}