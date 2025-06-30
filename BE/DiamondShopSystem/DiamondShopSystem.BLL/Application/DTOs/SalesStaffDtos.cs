using System;

namespace DiamondShopSystem.BLL.Application.DTOs
{
    public record UpdateOrderStatusRequest(
        Guid OrderId,
        string NewStatus
    );

    public record RegisterCustomerForVipRequest(
        Guid CustomerId,
        Guid VipStatusId
    );
}