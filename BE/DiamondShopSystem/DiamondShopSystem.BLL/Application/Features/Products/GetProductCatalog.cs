using MediatR;
using DiamondShopSystem.BLL.Application.DTOs;
using DiamondShopSystem.BLL.Application.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DiamondShopSystem.BLL.Application.Features.Products
{
    public class GetProductCatalogQuery : IRequest<IEnumerable<ProductDto>>
    {
        public string SearchTerm { get; set; }
        public Guid? CategoryId { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public string Color { get; set; }
        public string Clarity { get; set; }
        public string Cut { get; set; }
        public decimal? MinCarat { get; set; }
        public decimal? MaxCarat { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    public class GetProductCatalogQueryHandler : IRequestHandler<GetProductCatalogQuery, IEnumerable<ProductDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetProductCatalogQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<ProductDto>> Handle(GetProductCatalogQuery request, CancellationToken cancellationToken)
        {
            // In a real application, you would build a specification based on the query parameters
            // For simplicity, this example fetches all products and filters in memory (not recommended for large datasets)
            var products = await _unitOfWork.Products.ListAllAsync();

            // Apply filters (example, should be done at the repository/specification level)
            if (!string.IsNullOrEmpty(request.SearchTerm))
            {
                products = products.Where(p => p.Name.Contains(request.SearchTerm) || p.Description.Contains(request.SearchTerm)).ToList();
            }
            if (request.CategoryId.HasValue)
            {
                products = products.Where(p => p.CategoryId == request.CategoryId.Value).ToList();
            }
            if (request.MinPrice.HasValue)
            {
                products = products.Where(p => p.BasePrice >= request.MinPrice.Value).ToList();
            }
            if (request.MaxPrice.HasValue)
            {
                products = products.Where(p => p.BasePrice <= request.MaxPrice.Value).ToList();
            }
            // Add more filters for DiamondProperties, etc.

            // Pagination
            products = products.Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize).ToList();

            var productDtos = new List<ProductDto>();
            foreach (var product in products)
            {
                var category = await _unitOfWork.Categories.GetByIdAsync(product.CategoryId); // Assuming Categories repository exists
                productDtos.Add(new ProductDto(
                    product.Id,
                    product.SKU,
                    product.Name,
                    product.Description,
                    product.BasePrice,
                    product.CategoryId,
                    category?.Name ?? string.Empty, // Ensure not null
                    product.DiamondProperties
                ));
            }

            return productDtos;
        }
    }
}