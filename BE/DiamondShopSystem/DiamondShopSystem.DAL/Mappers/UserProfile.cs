using AutoMapper;
using DiamondShopSystem.BLL.Domain.Entities;
using DiamondShopSystem.BLL.Application.DTOs;

namespace DiamondShopSystem.DAL.Mappers
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role != null ? src.Role.Name : null));
        }
    }
} 