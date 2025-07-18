using DiamondShopSystem.BLL.Handlers.Category.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondShopSystem.BLL.Services.Category
{
    public interface ICategoryService
    {
        
        Task<IEnumerable<CategoryGetResponseDto>> GetAllCategoriesAsync();
        Task<CategoryGetResponseDto> GetCategoryByIdAsync(Guid id);
        Task<CategoryUpdateResponseDto> UpdateCategoryAsync(Guid id, CategoryUpdateDto category);
        Task DeleteCategoryAsync(Guid id);
    }
}
