using DiamondShopSystem.BLL.Services.Email;
using DiamondShopSystem.BLL.Services.Implements.VNPayService.Library;
using DiamondShopSystem.BLL.Services.Implements.VNPayService.Models;
using DiamondShopSystem.BLL.Services.Interfaces;
using DiamondShopSystem.BLL.Services.VNPayService.Library;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Text.Json;

namespace DiamondShopSystem.BLL.Services.Implements.VNPayService.Services
{
    public class VnPayService : IVnPayService
    {
        private readonly ILogger<VnPayService> _logger;
        private readonly VNPaySetting _vnPaySetting;

        public VnPayService(ILogger<VnPayService> logger, IOptions<VNPaySetting> vnPaySetting)
        {
            _logger = logger;
            _vnPaySetting = vnPaySetting.Value;
        }

        public string CreatePaymentUrl(PaymentInformationModel model, HttpContext context)
        {
            try
            {
                _logger.LogInformation("Creating VNPay payment URL for order: {OrderInfo}", model.OrderDescription);

                // Validate required settings
                ValidateVnPaySettings();

                var timeZoneById = GetTimeZone();
                var timeNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZoneById);

                var pay = new VnPayLibrary();

                // Build payment request
                pay.AddRequestData("vnp_Version", _vnPaySetting.Version);
                pay.AddRequestData("vnp_Command", _vnPaySetting.Command);
                pay.AddRequestData("vnp_TmnCode", _vnPaySetting.TmnCode);
                pay.AddRequestData("vnp_Amount", ((long)model.Amount * 100).ToString());
                pay.AddRequestData("vnp_CreateDate", timeNow.ToString("yyyyMMddHHmmss"));
                pay.AddRequestData("vnp_CurrCode", _vnPaySetting.CurrCode);
                pay.AddRequestData("vnp_IpAddr", pay.GetIpAddress(context));
                pay.AddRequestData("vnp_Locale", _vnPaySetting.Locale);
                pay.AddRequestData("vnp_OrderInfo", $"{model.Name} {model.OrderDescription}");
                pay.AddRequestData("vnp_OrderType", "other");

                // Use the configured success return URL from settings instead of model
                // VNPay will redirect to this URL after payment processing
                var returnUrlWithParams = $"{_vnPaySetting.ReturnUrlSuccess}?success_url={_vnPaySetting.ReturnUrlSuccess}&fail_url={_vnPaySetting.ReturnUrlFail}";
                pay.AddRequestData("vnp_ReturnUrl", returnUrlWithParams);
                pay.AddRequestData("vnp_TxnRef", timeNow.Ticks.ToString());

                var paymentUrl = pay.CreateRequestUrl(_vnPaySetting.BaseUrl, _vnPaySetting.HashSecret);

                _logger.LogInformation("VNPay payment URL created successfully for amount: {Amount}", model.Amount);
                return paymentUrl;
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError(ex, "Missing required VNPay configuration: {Message}", ex.Message);
                throw new InvalidOperationException($"VNPay configuration error: {ex.Message}", ex);
            }
            catch (TimeZoneNotFoundException ex)
            {
                _logger.LogError(ex, "Invalid TimeZone configuration: {TimeZoneId}", _vnPaySetting.TimeZoneId);
                throw new InvalidOperationException($"Invalid TimeZone '{_vnPaySetting.TimeZoneId}'. Check available time zones on the server.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while creating VNPay payment URL for order: {OrderInfo}", model.OrderDescription);
                throw new InvalidOperationException("Failed to create payment URL. Please try again later.", ex);
            }
        }

        public PaymentResponseModel PaymentExecute(IQueryCollection collections)
        {
            try
            {
                _logger.LogInformation("Processing VNPay payment response");

                var pay = new VnPayLibrary();
                var response = pay.GetFullResponseData(collections, _vnPaySetting.HashSecret);

                _logger.LogInformation("VNPay payment response processed successfully. Transaction Ref: {TxnRef}",
                    collections["vnp_TxnRef"]);

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing VNPay payment response");
                throw new InvalidOperationException("Failed to process payment response.", ex);
            }
        }

        private void ValidateVnPaySettings()
        {
            if (string.IsNullOrEmpty(_vnPaySetting.TimeZoneId))
                throw new ArgumentNullException(nameof(_vnPaySetting.TimeZoneId), "TimeZoneId is not configured in VNPay settings");

            if (string.IsNullOrEmpty(_vnPaySetting.ReturnUrlSuccess))
                throw new ArgumentNullException(nameof(_vnPaySetting.ReturnUrlSuccess), "ReturnUrlSuccess is not configured in VNPay settings");

            if (string.IsNullOrEmpty(_vnPaySetting.ReturnUrlFail))
                throw new ArgumentNullException(nameof(_vnPaySetting.ReturnUrlFail), "ReturnUrlFail is not configured in VNPay settings");

            if (string.IsNullOrEmpty(_vnPaySetting.BaseUrl))
                throw new ArgumentNullException(nameof(_vnPaySetting.BaseUrl), "BaseUrl is not configured in VNPay settings");

            if (string.IsNullOrEmpty(_vnPaySetting.HashSecret))
                throw new ArgumentNullException(nameof(_vnPaySetting.HashSecret), "HashSecret is not configured in VNPay settings");

            if (string.IsNullOrEmpty(_vnPaySetting.TmnCode))
                throw new ArgumentNullException(nameof(_vnPaySetting.TmnCode), "TmnCode is not configured in VNPay settings");
        }

        private TimeZoneInfo GetTimeZone()
        {
            try
            {
                return TimeZoneInfo.FindSystemTimeZoneById(_vnPaySetting.TimeZoneId);
            }
            catch (TimeZoneNotFoundException)
            {
                throw new TimeZoneNotFoundException($"TimeZoneId '{_vnPaySetting.TimeZoneId}' is invalid. Check available time zones on the server.");
            }
        }
    }
}