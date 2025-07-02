using MediatR;
using DiamondShopSystem.BLL.Application.DTOs;
using DiamondShopSystem.BLL.Application.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DiamondShopSystem.BLL.Application.Features.Products
{
    public class UpdateProductCommand : IRequest<ProductDto>
    {
        public UpdateProductDto Product { get; set; }
    }

    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, ProductDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateProductCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ProductDto> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(request.Product.Id);

            if (product == null)
            {
                return null; // Or throw a NotFoundException
            }

            product.SKU = request.Product.SKU;
            product.Name = request.Product.Name;
            product.Description = request.Product.Description;
            product.BasePrice = request.Product.BasePrice;
            product.CategoryId = request.Product.CategoryId;
            product.DiamondProperties = request.Product.DiamondProperties;

            await _unitOfWork.Products.UpdateAsync(product);
            await _unitOfWork.CommitAsync();

            var category = await _unitOfWork.Categories.GetByIdAsync(product.CategoryId);

            return new ProductDto(
                product.Id,
                product.SKU,
                product.Name,
                product.Description,
                product.BasePrice,
                product.CategoryId,
                category?.Name,
                product.DiamondProperties
            );
        }
    }
}