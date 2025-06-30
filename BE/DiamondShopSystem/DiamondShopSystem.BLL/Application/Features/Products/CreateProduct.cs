using MediatR;
using DiamondShopSystem.BLL.Application.DTOs;
using DiamondShopSystem.BLL.Application.Interfaces;
using DiamondShopSystem.BLL.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace DiamondShopSystem.BLL.Application.Features.Products
{
    public class CreateProductCommand : IRequest<ProductDto>
    {
        public CreateProductDto Product { get; set; }
    }

    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ProductDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateProductCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ProductDto> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var product = new Product
            {
                Id = Guid.NewGuid(),
                SKU = request.Product.SKU,
                Name = request.Product.Name,
                Description = request.Product.Description,
                BasePrice = request.Product.BasePrice,
                CategoryId = request.Product.CategoryId,
                DiamondProperties = request.Product.DiamondProperties
            };

            await _unitOfWork.Products.AddAsync(product);
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