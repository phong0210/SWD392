using System;
using DiamondShopSystem.BLL.Handlers.User.DTOs;

namespace DiamondShopSystem.BLL.Services.Cache
{
    public interface IOtpCacheService
    {
        void StoreOtp(string email, string otp, DateTime expiration);
        (string otp, DateTime expiration)? GetOtp(string email);
        void RemoveOtp(string email);
        void StoreUserRegistrationData(string email, UserRegisterDto userData);
        UserRegisterDto? GetUserRegistrationData(string email);
        void RemoveUserRegistrationData(string email);
    }
}