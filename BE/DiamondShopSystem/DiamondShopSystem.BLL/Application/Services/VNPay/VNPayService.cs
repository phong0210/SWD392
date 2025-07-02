using DiamondShopSystem.BLL.Application.DTOs;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace DiamondShopSystem.BLL.Application.Services.VNPay
{
    public class VNPayService : IVNPayService
    {
        private readonly IConfiguration _configuration;
        private readonly string _tmnCode;
        private readonly string _hashSecret;
        private readonly string _vnpayUrl;
        private readonly string _apiUrl;

        public VNPayService(IConfiguration configuration)
        {
            _configuration = configuration;
            _tmnCode = _configuration["VNPay:TmnCode"];
            _hashSecret = _configuration["VNPay:HashSecret"];
            _vnpayUrl = _configuration["VNPay:Url"];
            _apiUrl = _configuration["VNPay:ApiUrl"];
        }

        public string CreatePaymentUrl(CreateVNPayPaymentRequest request)
        {
            var vnp_Params = new SortedList<string, string>();
            vnp_Params.Add("vnp_Version", "2.1.0");
            vnp_Params.Add("vnp_Command", "pay");
            vnp_Params.Add("vnp_TmnCode", _tmnCode);
            vnp_Params.Add("vnp_Amount", (request.Amount * 100).ToString()); // Amount in cents
            vnp_Params.Add("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss"));
            vnp_Params.Add("vnp_CurrCode", "VND");
            vnp_Params.Add("vnp_IpAddr", "127.0.0.1"); // Get actual IP address
            vnp_Params.Add("vnp_Locale", "vn");
            vnp_Params.Add("vnp_OrderInfo", request.OrderInfo);
            vnp_Params.Add("vnp_OrderType", "other");
            vnp_Params.Add("vnp_ReturnUrl", request.ReturnUrl);
            vnp_Params.Add("vnp_TxnRef", request.OrderId.ToString());

            string signData = string.Join("&", vnp_Params.Select(kv => $"{kv.Key}={HttpUtility.UrlEncode(kv.Value)}"));
            string vnp_SecureHash = HmacSHA512(_hashSecret, signData);

            return $"{_vnpayUrl}?{signData}&vnp_SecureHash={vnp_SecureHash}";
        }

        public bool ProcessPaymentCallback(VNPayCallbackRequest request)
        {
            var vnp_Params = new SortedList<string, string>();
            foreach (var prop in request.GetType().GetProperties())
            {
                var value = prop.GetValue(request)?.ToString();
                if (value != null && prop.Name != "vnp_SecureHash")
                {
                    vnp_Params.Add(prop.Name, value);
                }
            }

            string signData = string.Join("&", vnp_Params.Select(kv => $"{kv.Key}={HttpUtility.UrlEncode(kv.Value)}"));
            string expectedHash = HmacSHA512(_hashSecret, signData);

            return expectedHash.Equals(request.vnp_SecureHash, StringComparison.OrdinalIgnoreCase);
        }

        private string HmacSHA512(string key, string inputData)
        {
            var hash = new StringBuilder();
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            byte[] inputBytes = Encoding.UTF8.GetBytes(inputData);
            using (var hmac = new HMACSHA512(keyBytes))
            {
                byte[] hashBytes = hmac.ComputeHash(inputBytes);
                foreach (var b in hashBytes)
                {
                    hash.Append(b.ToString("X2"));
                }
            }
            return hash.ToString();
        }
    }
}