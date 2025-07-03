using System;
using System.ComponentModel.DataAnnotations;

namespace DiamondShopSystem.DAL.Entities
{
    public class Delivery
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public Order Order { get; set; } = default!;
        public Guid DeliveryStaffId { get; set; }
        public StaffProfile DeliveryStaff { get; set; } = default!;
        public DateTime? DispatchTime { get; set; }
        public DateTime? DeliveryTime { get; set; }

        [StringLength(255)]
        public string ShippingAddress { get; set; } = string.Empty;

        public int Status { get; set; }
    }
} 