using System;
using System.Collections.Generic;
using DiamondShopSystem.BLL.Domain.ValueObjects;
using DiamondShopSystem.BLL.Domain.Entities;
using DiamondShopSystem.BLL.Domain.Enums;

namespace DiamondShopSystem.BLL.Application.DTOs
{
    public record RegisterUserRequest(
        string FullName,
        string Email,
        string Phone,
        string Password
    );

    public record LoginRequest(
        string Email,
        string Password
    );

    public record LoginResponse(
        string Token,
        Guid UserId,
        string FullName,
        string Email,
        string Role
    );

    public record OrderDto(
        Guid Id,
        Guid CustomerId,
        DateTime OrderDate,
        OrderStatus Status,
        decimal TotalAmount,
        Address ShippingAddress,
        List<OrderDetailDto> OrderDetails
    );

    public record OrderDetailDto(
        Guid ProductId,
        int Quantity,
        decimal PriceAtTimeOfPurchase
    );

    public record OrderStatusDto(
        Guid OrderId,
        OrderStatus Status,
        DateTime OrderDate,
        decimal TotalAmount,
        Address ShippingAddress,
        DeliveryStatus? DeliveryStatus,
        DateTime? DispatchedDate,
        DateTime? DeliveredDate
    );

    public record OrderSummaryDto(
        Guid Id,
        DateTime OrderDate,
        OrderStatus Status,
        decimal TotalAmount,
        int ItemCount,
        DeliveryStatus? DeliveryStatus
    );

    public record PromotionDto(
        Guid Id,
        string Code,
        string Description,
        decimal DiscountPercentage,
        DateTime StartDate,
        DateTime EndDate,
        Guid ProductId,
        string ProductName
    );

    public record PlaceOrderRequest(
        Guid CustomerId,
        List<OrderItemDto> Items,
        Address ShippingAddress
    );

    public record UserDto(
        Guid Id,
        string FullName,
        string Email,
        string Phone,
        string RoleName,
        bool IsActive
    );

    public record CreateVNPayPaymentRequest(
        Guid OrderId,
        decimal Amount,
        string OrderInfo,
        string ReturnUrl
    );

    public record VNPayCallbackRequest(
        string vnp_Amount,
        string vnp_BankCode,
        string vnp_BankTranNo,
        string vnp_CardType,
        string vnp_OrderInfo,
        string vnp_PayDate,
        string vnp_ResponseCode,
        string vnp_TmnCode,
        string vnp_TransactionNo,
        string vnp_TransactionStatus,
        string vnp_TxnRef,
        string vnp_SecureHash
    );

    public record HandleOrderFailureRequest(
        Guid OrderId,
        string Reason
    );

    public record ForgotPasswordRequest(
        string Email
    );

    public record ForgotPasswordResponse(
        string Message,
        bool IsSuccess
    );

    public record ResetPasswordRequest(
        string Token,
        string NewPassword
    );

    public record ResetPasswordResponse(
        string Message,
        bool IsSuccess
    );
}