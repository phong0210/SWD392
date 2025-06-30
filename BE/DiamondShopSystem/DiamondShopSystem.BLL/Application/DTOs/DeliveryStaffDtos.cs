using System;
using System.Collections.Generic;

namespace DiamondShopSystem.BLL.Application.DTOs
{
    public record DeliveryDto(
        Guid Id,
        Guid OrderId,
        string OrderStatus,
        string ShippingAddress,
        string CustomerName,
        string CustomerPhone,
        string DeliveryStatus,
        DateTime? DispatchedDate,
        DateTime? DeliveredDate
    );

    public record UpdateShipmentStatusRequest(
        Guid DeliveryId,
        string NewStatus
    );
}