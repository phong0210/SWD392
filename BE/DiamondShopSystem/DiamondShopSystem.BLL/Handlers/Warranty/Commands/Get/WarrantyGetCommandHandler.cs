using DiamondShopSystem.BLL.Handlers.Product.Commands.Get;
using DiamondShopSystem.BLL.Handlers.Product.DTOs;
using DiamondShopSystem.BLL.Handlers.Warranty.DTOs;
using DiamondShopSystem.BLL.Services.Product;
using DiamondShopSystem.BLL.Services.Warranty;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DiamondShopSystem.BLL.Handlers.Warranty.Commands.Get
{
    public class WarrantyGetCommandHandler : IRequestHandler<WarrantyGetCommand, WarrantyGetResponseDto>
    {
        private readonly IWarrantyService _warrantyService;

        public WarrantyGetCommandHandler(IWarrantyService productService)
        {
            _warrantyService = productService;
        }

        public async Task<WarrantyGetResponseDto> Handle(WarrantyGetCommand request, CancellationToken cancellationToken)
        {
            return await _warrantyService.GetWarrantyByIdAsync(request.WarrantyId);
        }
    }
}