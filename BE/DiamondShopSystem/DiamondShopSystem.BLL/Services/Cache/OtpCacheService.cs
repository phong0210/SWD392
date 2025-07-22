using System;
using System.Collections.Concurrent;
using DiamondShopSystem.BLL.Handlers.User.DTOs;

namespace DiamondShopSystem.BLL.Services.Cache
{
    public class OtpCacheService : IOtpCacheService
    {
        private static readonly ConcurrentDictionary<string, (string otp, DateTime expiration)> _otpStorage = new ConcurrentDictionary<string, (string otp, DateTime expiration)>();
        private static readonly ConcurrentDictionary<string, UserRegisterDto> _userRegistrationStorage = new ConcurrentDictionary<string, UserRegisterDto>();

        public void StoreOtp(string email, string otp, DateTime expiration)
        {
            _otpStorage[email] = (otp, expiration);
        }

        public (string otp, DateTime expiration)? GetOtp(string email)
        {
            if (_otpStorage.TryGetValue(email, out var otpData))
            {
                return otpData;
            }
            return null;
        }

        public void RemoveOtp(string email)
        {
            _otpStorage.TryRemove(email, out _);
        }

        public void StoreUserRegistrationData(string email, UserRegisterDto userData)
        {
            _userRegistrationStorage[email] = userData;
        }

        public UserRegisterDto? GetUserRegistrationData(string email)
        {
            if (_userRegistrationStorage.TryGetValue(email, out var userData))
            {
                return userData;
            }
            return null;
        }

        public void RemoveUserRegistrationData(string email)
        {
            _userRegistrationStorage.TryRemove(email, out _);
        }
    }
}
