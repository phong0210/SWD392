using System;

namespace DiamondShopSystem.BLL.Handlers.Delivery.DTOs
{
    public class UpdateDeliveryDto
    {
        public DateTime? DispatchTime { get; set; }
        public DateTime? DeliveryTime { get; set; }
        public string? ShippingAddress { get; set; }
        public int Status { get; set; }
    }
}