
using AutoMapper;
using DiamondShopSystem.BLL.Domain.Entities;
using DiamondShopSystem.BLL.Application.DTOs;

namespace DiamondShopSystem.DAL.Mappers
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<CreateProductRequest, Product>();
            CreateMap<UpdateProductRequest, Product>();
            CreateMap<Product, ProductResponse>();
        }
    }
}
