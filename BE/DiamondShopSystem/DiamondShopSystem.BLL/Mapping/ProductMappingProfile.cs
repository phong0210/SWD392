using AutoMapper;
using DiamondShopSystem.BLL.Handlers.Product.DTOs;
using DiamondShopSystem.BLL.Handlers.User.DTOs;
using DiamondShopSystem.DAL.Entities;

public class ProductMappingProfile : Profile
{
    public ProductMappingProfile()
    {
        CreateMap<Product, ProductInfoDto>();

        CreateMap<Product, ProductCreateDto>();
        CreateMap<ProductCreateDto, Product>();


    }
}