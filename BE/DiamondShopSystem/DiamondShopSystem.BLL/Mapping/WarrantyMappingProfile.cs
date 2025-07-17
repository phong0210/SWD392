using AutoMapper;
using DiamondShopSystem.DAL.Entities;
using DiamondShopSystem.BLL.Handlers.Warranty.DTOs;

namespace DiamondShopSystem.BLL.Mapping
{
    public class WarrantyMappingProfile : Profile
    {
        public WarrantyMappingProfile()
        {
            CreateMap<Warranty, WarrantyInfoDto>();
            CreateMap<WarrantyCreateDto, Warranty>();
            CreateMap<WarrantyUpdateDto, Warranty>();
        }
    }
}