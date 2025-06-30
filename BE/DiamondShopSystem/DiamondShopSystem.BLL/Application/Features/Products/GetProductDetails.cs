using MediatR;
using DiamondShopSystem.BLL.Application.DTOs;
using DiamondShopSystem.BLL.Application.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DiamondShopSystem.BLL.Application.Features.Products
{
    public class GetProductDetailsQuery : IRequest<ProductDto>
    {
        public Guid Id { get; set; }
    }

    public class GetProductDetailsQueryHandler : IRequestHandler<GetProductDetailsQuery, ProductDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetProductDetailsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ProductDto> Handle(GetProductDetailsQuery request, CancellationToken cancellationToken)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(request.Id);

            if (product == null)
            {
                return null; // Or throw a NotFoundException
            }

            var category = await _unitOfWork.Categories.GetByIdAsync(product.CategoryId);

            return new ProductDto(
                product.Id,
                product.SKU,
                product.Name,
                product.Description,
                product.BasePrice,
                product.CategoryId,
                category?.Name ?? string.Empty,
                product.DiamondProperties
            );
        }
    }
}