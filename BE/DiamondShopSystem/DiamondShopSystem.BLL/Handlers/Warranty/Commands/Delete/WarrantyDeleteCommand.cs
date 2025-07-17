using MediatR;
using System;

namespace DiamondShopSystem.BLL.Handlers.Warranty.Commands.Delete
{
    public class WarrantyDeleteCommand : IRequest<bool>
    {
        public Guid WarrantyId { get; }
        public WarrantyDeleteCommand(Guid warrantyId)
        {
            WarrantyId = warrantyId;
        }
    }
}