using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using DiamondShopSystem.BLL.Services.Product;
using DiamondShopSystem.BLL.Handlers.Product.DTOs;

namespace DiamondShopSystem.BLL.Handlers.Product.Commands.Get
{
    public class ProductGetCommandHandler : IRequestHandler<ProductGetCommand, ProductGetResponseDto>
    {
        private readonly IProductService _productService;

        public ProductGetCommandHandler(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<ProductGetResponseDto> Handle(ProductGetCommand request, CancellationToken cancellationToken)
        {
            return await _productService.GetProductByIdAsync(request.ProductId);
        }
    }
}