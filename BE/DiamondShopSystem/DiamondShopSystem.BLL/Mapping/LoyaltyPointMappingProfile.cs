using AutoMapper;
using DiamondShopSystem.BLL.Handlers.LoyaltyPoints.DTOs;
using DiamondShopSystem.DAL.Entities;

namespace DiamondShopSystem.BLL.Mapping
{
    public class LoyaltyPointMappingProfile : Profile
    {
        public LoyaltyPointMappingProfile()
        {
            CreateMap<LoyaltyPoints, LoyaltyPointDto>().ReverseMap();
        }
    }
}
