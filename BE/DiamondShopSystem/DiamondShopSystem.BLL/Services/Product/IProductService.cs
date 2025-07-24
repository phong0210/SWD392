using DiamondShopSystem.BLL.Handlers.Product.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondShopSystem.BLL.Services.Product
{
    public interface IProductService
    {
        Task<IEnumerable<ProductGetResponseDto>> GetAllProductsAsync();
        Task<ProductGetResponseDto> GetProductByIdAsync(Guid productId);
        Task<ProductUpdateResponseDto> UpdateProductAsync(Guid productId, ProductUpdateDto updateDto);
        
        Task DeleteProductAsync(Guid productId, Boolean status);
    }
}