using AutoMapper;
using DiamondShopSystem.BLL.Handlers.Category.DTOs;
using DiamondShopSystem.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondShopSystem.BLL.Services.Category
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public Task DeleteCategoryAsync(Guid id)
        {
            var categoryRepo = _unitOfWork.Repository<DiamondShopSystem.DAL.Entities.Category>();
            var categoryEntity = categoryRepo.GetByIdAsync(id);
            if (categoryEntity == null)
            {
                return Task.FromResult(new CategoryGetResponseDto
                {
                    Success = false,
                    Error = "Category not found"
                });
            }
            categoryRepo.Remove(categoryEntity.Result);
            return _unitOfWork.SaveChangesAsync();
        }

        public Task<IEnumerable<CategoryGetResponseDto>> GetAllCategoriesAsync()
        {
            var categoryRepo = _unitOfWork.Repository<DiamondShopSystem.DAL.Entities.Category>();
            var categories = categoryRepo.GetAllAsync();
            if (categories == null || !categories.Result.Any())
            {
                return Task.FromResult(new List<CategoryGetResponseDto>
                {
                    new CategoryGetResponseDto
                    {
                        Success = false,
                        Error = "No categories found"
                    }
                }.AsEnumerable());
            }
            var categoryDtos = categories.Result.Select(category => new CategoryGetResponseDto
            {
                Success = true,
                Category = _mapper.Map<CategoryInfoDto>(category)
            });
            return Task.FromResult(categoryDtos.AsEnumerable());

        }

        public Task<CategoryGetResponseDto> GetCategoryByIdAsync(Guid id)
        {
           var categoryRepo = _unitOfWork.Repository<DiamondShopSystem.DAL.Entities.Category>();
            var categoryEntity = categoryRepo.GetByIdAsync(id);
            if (categoryEntity == null)
            {
                return Task.FromResult(new CategoryGetResponseDto
                {
                    Success = false,
                    Error = "Category not found"
                });
            }
            var categoryDto = new CategoryGetResponseDto
            {
                Success = true,
                Category = _mapper.Map<CategoryInfoDto>(categoryEntity.Result)
            };
            return Task.FromResult(categoryDto);
        }

        public async Task<CategoryUpdateResponseDto> UpdateCategoryAsync(Guid id, CategoryUpdateDto category)
        {
            var categoryRepo = _unitOfWork.Repository<DiamondShopSystem.DAL.Entities.Category>();
            var categoryEntity = categoryRepo.GetByIdAsync(id);
            if (categoryEntity == null)
            {
                return (new CategoryUpdateResponseDto
                {
                    Success = false,
                    Error = "Category not found"

                });
            }
            categoryEntity.Result.Name = category.Name;
            categoryEntity.Result.Description = category.Description;
            categoryRepo.Update(categoryEntity.Result);
            await _unitOfWork.SaveChangesAsync();
            
            var uppdatedCategory = await categoryRepo.GetByIdAsync(id);
            if (uppdatedCategory == null)
            {
                return new CategoryUpdateResponseDto
                {
                    Success = false,
                    Error = "Failed to update category"
                };
            }

            return new CategoryUpdateResponseDto
            {
                Success = true,
                category = _mapper.Map<CategoryInfoDto>(categoryEntity.Result)
            };


        }
    }
}
