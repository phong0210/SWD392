
using DiamondShopSystem.BLL.Application.DTOs;

namespace DiamondShopSystem.BLL.Services.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductResponse>> GetAllProductsAsync();
        Task<ProductResponse> GetProductByIdAsync(Guid id);
        Task<ProductResponse> CreateProductAsync(CreateProductRequest request);
        Task<ProductResponse> UpdateProductAsync(Guid id, UpdateProductRequest request);
        Task<bool> DeleteProductAsync(Guid id);
    }
}
