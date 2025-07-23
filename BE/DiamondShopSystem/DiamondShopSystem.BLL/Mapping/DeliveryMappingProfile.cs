
using AutoMapper;
using DiamondShopSystem.BLL.Handlers.Delivery.DTOs;
using DiamondShopSystem.DAL.Entities;

namespace DiamondShopSystem.BLL.Mapping
{
    public class DeliveryMappingProfile : Profile
    {
        public DeliveryMappingProfile()
        {
            CreateMap<Delivery, DeliveryDto>().ReverseMap();
            CreateMap<Delivery, CreateDeliveryDto>().ReverseMap();
            CreateMap<Delivery, UpdateDeliveryDto>().ReverseMap();
        }
    }
}
