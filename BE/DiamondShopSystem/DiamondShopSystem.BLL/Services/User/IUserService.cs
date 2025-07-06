using System;
using System.Threading.Tasks;
using DiamondShopSystem.BLL.Handlers.User.DTOs;

namespace DiamondShopSystem.BLL.Services.User
{
    public interface IUserService
    {
        Task<UserGetResponseDto> GetUserByIdAsync(Guid userId);
        Task<UserUpdateResponseDto> UpdateUserAccountAsync(Guid userId, UserUpdateDto updateDto);
    }
} 