using AutoMapper;
using DiamondShopSystem.BLL.Handlers.Vip.DTOs;
using DiamondShopSystem.DAL.Entities;

namespace DiamondShopSystem.BLL.Mapping
{
    public class VipMappingProfile : Profile
    {
        public VipMappingProfile()
        {
            CreateMap<Vip, VipDto>().ReverseMap();
            CreateMap<VipCreateRequestDto, Vip>();
            CreateMap<VipUpdateRequestDto, Vip>();
        }
    }
}