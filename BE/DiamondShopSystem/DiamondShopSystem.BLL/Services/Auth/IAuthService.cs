using System;
using DiamondShopSystem.DAL.Entities;

namespace DiamondShopSystem.BLL.Services.Auth
{
    public interface IAuthService
    {
        string HashPassword(string password);
        string GenerateJwtToken(DiamondShopSystem.DAL.Entities.User user);
        bool ValidatePassword(string password, string hashedPassword);
    }
} 