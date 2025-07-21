using System;
using MediatR;
using DiamondShopSystem.BLL.Handlers.Product.DTOs;
using DiamondShopSystem.BLL.Handlers.Warranty.DTOs;

namespace DiamondShopSystem.BLL.Handlers.Product.Commands.Get
{
    public class WarrantyGetCommand : IRequest<WarrantyGetResponseDto>
    {
        public Guid WarrantyId { get; }

        public WarrantyGetCommand(Guid warrantyId)
        {
            WarrantyId = warrantyId;
        }
    }
}