using System;
using DiamondShopSystem.DAL.Entities;

namespace DiamondShopSystem.BLL.Services.Auth
{
    public interface IAuthService
    {
        string HashPassword(string password);
        Task<string> GenerateJwtTokenAsync(DiamondShopSystem.DAL.Entities.User user); 
        bool ValidatePassword(string password, string hashedPassword);
    }
} 