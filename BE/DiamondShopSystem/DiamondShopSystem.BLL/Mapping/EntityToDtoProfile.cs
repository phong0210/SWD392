using AutoMapper;
using DiamondShopSystem.DAL.Entities;
using DiamondShopSystem.BLL.Handlers.User.DTOs;
using DiamondShopSystem.BLL.Handlers.Auth.DTOs;

namespace DiamondShopSystem.BLL.Mapping
{
    public class EntityToDtoProfile : Profile
    {
        public EntityToDtoProfile()
        {
            // Auth mappings
            CreateMap<User, LoginResponseDto>();
            
            // User mappings
            CreateMap<User, UserDto>();
            CreateMap<User, UserCreateDto>();
            CreateMap<UserCreateDto, User>();
            
            // User Get mappings
            CreateMap<User, UserAccountInfoDto>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.Status));
        }
    }
} 