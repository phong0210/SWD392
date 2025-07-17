using System;

namespace DiamondShopSystem.BLL.Models
{
    public class DeliveryDto
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public DateTime? DispatchTime { get; set; }
        public DateTime? DeliveryTime { get; set; }
        public string? ShippingAddress { get; set; }
        public int Status { get; set; }
    }
}