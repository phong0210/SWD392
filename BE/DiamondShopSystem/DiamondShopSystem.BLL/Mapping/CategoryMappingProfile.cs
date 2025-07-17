using AutoMapper;
using DiamondShopSystem.DAL.Entities;
using DiamondShopSystem.BLL.Handlers.Category.DTOs;

namespace DiamondShopSystem.BLL.Mapping
{
    public class CategoryMappingProfile : Profile
    {
        public CategoryMappingProfile()
        {
            CreateMap<Category, CategoryInfoDto>();
            CreateMap<CategoryCreateDto, Category>();
            CreateMap<CategoryUpdateDto, Category>();
        }
    }
}