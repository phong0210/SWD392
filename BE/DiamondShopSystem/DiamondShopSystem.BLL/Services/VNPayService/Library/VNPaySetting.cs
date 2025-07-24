using System;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static System.Net.WebRequestMethods;
using static System.Runtime.CompilerServices.RuntimeHelpers;

namespace DiamondShopSystem.BLL.Services.VNPayService.Library
{
    public class VNPaySetting
    {
        public string TmnCode { get; set; }
        public string HashSecret { get; set; }
        public string BaseUrl { get; set; }
        public string Version { get; set; }
        public string Command { get; set; }
        public string CurrCode { get; set; }
        public string Locale { get; set; }
        public string TimeZoneId { get; set; }
        public string ReturnUrlSuccess { get; set; }
        public string ReturnUrlFail { get; set; }
    }
}