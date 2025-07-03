using AutoMapper;
using DiamondShopSystem.DAL.Entities;
using DiamondShopSystem.BLL.Handlers.User;
using DiamondShopSystem.BLL.Handlers.Auth;

namespace DiamondShopSystem.BLL.Mapping
{
    public class EntityToDtoProfile : Profile
    {
        public EntityToDtoProfile()
        {
            // Example mappings (add DTOs as you create them)
            CreateMap<User, LoginResponseDto>();
            CreateMap<User, UserCreateDto>();
            CreateMap<User, UserCreateValidator>();
            CreateMap<UserCreateDto, User>();
        }
    }
} 