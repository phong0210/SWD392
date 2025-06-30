using DiamondShopSystem.BLL.Domain.ValueObjects;
using System;
using System.Collections.Generic;

namespace DiamondShopSystem.BLL.Application.DTOs
{
    public record AddToCartRequest(
        Guid ProductId,
        int Quantity
    );

    public record CartItemDto(
        Guid ProductId,
        string ProductName,
        int Quantity,
        decimal UnitPrice,
        decimal TotalPrice
    );

    public record OrderItemDto(
        Guid ProductId,
        int Quantity
    );

    public record CartDto(
        Guid CustomerId,
        List<CartItemDto> Items,
        decimal TotalAmount
    );
}