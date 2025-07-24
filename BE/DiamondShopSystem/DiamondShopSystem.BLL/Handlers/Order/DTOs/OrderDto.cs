
using System;
using System.Collections.Generic;

namespace DiamondShopSystem.BLL.Handlers.Order.DTOs
{
    // Represents a single item within an order
    public class OrderItemDto
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public double UnitPrice { get; set; }
    }

    // Data needed to create a new order
    public class CreateOrderDto
    {
        public Guid CustomerId { get; set; }
        public string SaleStaff { get; set; } = string.Empty;
        public List<OrderItemDto> OrderItems { get; set; } = new List<OrderItemDto>();
        public string PaymentMethod { get; set; } = string.Empty;
        public double TotalPrice { get; set; }
    }

    // The response after creating an order
    public class CreateOrderResponseDto
    {
        public bool Success { get; set; }
        public Guid? OrderId { get; set; }
        public string? Error { get; set; }
    }

    // Data needed to update an order
    public class UpdateOrderDto
    {
        public int Status { get; set; }
        public string SaleStaff { get; set; } = string.Empty;
        public bool VipApplied { get; set; }
    }

    // The response after updating an order
    public class UpdateOrderResponseDto
    {
        public bool Success { get; set; }
        public string? Error { get; set; }
    }

    // The response after deleting an order
    public class DeleteOrderResponseDto
    {
        public bool Success { get; set; }
        public string? Error { get; set; }
    }

    public class OrderDetailResponseDto
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public double UnitPrice { get; set; }
        public int Quantity { get; set; }
    }

    public class PaymentResponseDto
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public string Method { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public double Amount { get; set; }
        public int Status { get; set; }
    }

    public class DeliveryResponseDto
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public DateTime? DispatchTime { get; set; }
        public DateTime? DeliveryTime { get; set; }
        public string ShippingAddress { get; set; } = string.Empty;
        public int Status { get; set; }
    }

    public class OrderResponseDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public double TotalPrice { get; set; }
        public DateTime OrderDate { get; set; }
        public bool VipApplied { get; set; }
        public int Status { get; set; }
        public string SaleStaff { get; set; } = string.Empty;

        public List<OrderDetailResponseDto> OrderDetails { get; set; } = new List<OrderDetailResponseDto>();
        public DeliveryResponseDto? Delivery { get; set; }
        public List<PaymentResponseDto> Payments { get; set; } = new List<PaymentResponseDto>();
    }

    public class GetRevenueSummaryResponseDto
    {
        public bool Success { get; set; }
        public double TotalRevenue { get; set; }
        public string? Error { get; set; }
    }

    public class GetOrderByIdResponseDto
    {
        public bool Success { get; set; }
        public OrderResponseDto? Order { get; set; }
        public string? Error { get; set; }
    }

    public class GetOrderRelationsResponseDto
    {
        public bool Success { get; set; }
        public DiamondShopSystem.BLL.Handlers.User.DTOs.UserDto? User { get; set; }
        public string? Error { get; set; }
    }
}
