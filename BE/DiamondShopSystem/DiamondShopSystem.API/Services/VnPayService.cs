using DiamondShopSystem.API.Models;
using DiamondShopSystem.API.Utilities;
using DiamondShopSystem.API.Enums;
using Microsoft.AspNetCore.Http;
using System.Globalization;

namespace DiamondShopSystem.API.Services
{
    public interface IVnPayService
    {
        string CreatePaymentUrl(Guid orderId, decimal amount, string ipAddress);
        PaymentResult ProcessPaymentCallback(IQueryCollection collections);
    }

    public class VnPayService : IVnPayService
    {
        private readonly string _tmnCode;
        private readonly string _hashSecret;
        private readonly string _baseUrl;
        private readonly string _callbackUrl;
        private readonly string _version;
        private readonly string _orderType;

        public VnPayService(IConfiguration configuration)
        {
            _tmnCode = configuration["VnPay:TmnCode"];
            _hashSecret = configuration["VnPay:HashSecret"];
            _baseUrl = configuration["VnPay:BaseUrl"];
            _callbackUrl = configuration["VnPay:CallbackUrl"];
            _version = configuration["VnPay:Version"] ?? "2.1.0";
            _orderType = configuration["VnPay:OrderType"] ?? "other";
        }

        public string CreatePaymentUrl(Guid orderId, decimal amount, string ipAddress)
        {
            if (amount < 5000 || amount > 1000000000)
            {
                throw new ArgumentException("Số tiền thanh toán phải nằm trong khoảng 5.000 (VND) đến 1.000.000.000 (VND).");
            }

            if (string.IsNullOrEmpty(ipAddress))
            {
                throw new ArgumentException("Không được để trống địa chỉ IP.");
            }

            var paymentId = GeneratePaymentId(); // Self-generated payment ID
            var description = $"Payment for Order ID: {orderId}";

            var request = new PaymentRequest
            {
                PaymentId = paymentId,
                Money = amount,
                Description = description,
                IpAddress = ipAddress,
                CreatedDate = DateTime.Now,
                Currency = Currency.VND, // Assuming VND as default currency
                Language = DisplayLanguage.Vietnamese // Assuming Vietnamese as default language
            };

            var helper = new PaymentHelper();
            helper.AddRequestData("vnp_Version", _version);
            helper.AddRequestData("vnp_Command", "pay");
            helper.AddRequestData("vnp_TmnCode", _tmnCode);
            long amountInLong = (long)(request.Money * 100);
            helper.AddRequestData("vnp_Amount", ((long)(((decimal)request.Money) * 100M)).ToString());
            helper.AddRequestData("vnp_CreateDate", request.CreatedDate.ToString("yyyyMMddHHmmss"));
            helper.AddRequestData("vnp_CurrCode", request.Currency.ToString().ToUpper());
            helper.AddRequestData("vnp_IpAddr", request.IpAddress);
            helper.AddRequestData("vnp_Locale", EnumHelper.GetDescription(request.Language));
            helper.AddRequestData("vnp_BankCode", string.Empty); // Allow any bank
            helper.AddRequestData("vnp_OrderInfo", request.Description.Trim());
            helper.AddRequestData("vnp_OrderType", _orderType);
            helper.AddRequestData("vnp_ReturnUrl", _callbackUrl);
            helper.AddRequestData("vnp_TxnRef", request.PaymentId.ToString());

            return helper.GetPaymentUrl(_baseUrl, _hashSecret);
        }

        public PaymentResult ProcessPaymentCallback(IQueryCollection parameters)
        {
            var responseData = parameters
                .Where(kv => !string.IsNullOrEmpty(kv.Key) && kv.Key.StartsWith("vnp_"))
                .ToDictionary(kv => kv.Key, kv => kv.Value.ToString());

            var vnp_BankCode = responseData.GetValueOrDefault("vnp_BankCode");
            var vnp_BankTranNo = responseData.GetValueOrDefault("vnp_BankTranNo");
            var vnp_CardType = responseData.GetValueOrDefault("vnp_CardType");
            var vnp_PayDate = responseData.GetValueOrDefault("vnp_PayDate");
            var vnp_OrderInfo = responseData.GetValueOrDefault("vnp_OrderInfo");
            var vnp_TransactionNo = responseData.GetValueOrDefault("vnp_TransactionNo");
            var vnp_ResponseCode = responseData.GetValueOrDefault("vnp_ResponseCode");
            var vnp_TransactionStatus = responseData.GetValueOrDefault("vnp_TransactionStatus");
            var vnp_TxnRef = responseData.GetValueOrDefault("vnp_TxnRef");
            var vnp_SecureHash = responseData.GetValueOrDefault("vnp_SecureHash");

            if (string.IsNullOrEmpty(vnp_BankCode)
                || string.IsNullOrEmpty(vnp_OrderInfo)
                || string.IsNullOrEmpty(vnp_TransactionNo)
                || string.IsNullOrEmpty(vnp_ResponseCode)
                || string.IsNullOrEmpty(vnp_TransactionStatus)
                || string.IsNullOrEmpty(vnp_TxnRef)
                || string.IsNullOrEmpty(vnp_SecureHash))
            {
                throw new ArgumentException("Không đủ dữ liệu để xác thực giao dịch");
            }

            var helper = new PaymentHelper();
            foreach (var (key, value) in responseData)
            {
                if (!key.Equals("vnp_SecureHash"))
                {
                    helper.AddResponseData(key, value);
                }
            }

            var responseCode = (ResponseCode)sbyte.Parse(vnp_ResponseCode);
            var transactionStatusCode = (TransactionStatusCode)sbyte.Parse(vnp_TransactionStatus);

            return new PaymentResult
            {
                PaymentId = long.Parse(vnp_TxnRef),
                VnpayTransactionId = long.Parse(vnp_TransactionNo),
                IsSuccess = transactionStatusCode == TransactionStatusCode.Code_00 && responseCode == ResponseCode.Code_00 && helper.IsSignatureCorrect(vnp_SecureHash, _hashSecret),
                Description = vnp_OrderInfo,
                PaymentMethod = string.IsNullOrEmpty(vnp_CardType)
                    ? "Không xác định"
                    : vnp_CardType,
                Timestamp = string.IsNullOrEmpty(vnp_PayDate)
                    ? DateTime.Now
                    : DateTime.ParseExact(vnp_PayDate, "yyyyMMddHHmmss", CultureInfo.InvariantCulture),
                TransactionStatus = new TransactionStatus
                {
                    Code = transactionStatusCode,
                    Description = EnumHelper.GetDescription(transactionStatusCode)
                },
                PaymentResponse = new PaymentResponse
                {
                    Code = responseCode,
                    Description = EnumHelper.GetDescription(responseCode)
                },
                BankingInfor = new BankingInfor
                {
                    BankCode = vnp_BankCode,
                    BankTransactionId = string.IsNullOrEmpty(vnp_BankTranNo)
                        ? "Không xác định"
                        : vnp_BankTranNo,
                }
            };
        }

        private long GeneratePaymentId()
        {
            // Generate a unique payment ID, e.g., using timestamp or a GUID converted to a long
            // For simplicity, using a timestamp here. In a real application, ensure uniqueness and collision avoidance.
            return DateTime.Now.Ticks;
        }
    }
}
