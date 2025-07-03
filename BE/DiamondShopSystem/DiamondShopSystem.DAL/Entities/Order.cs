using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiamondShopSystem.DAL.Entities
{
    public class Order
    {
        [Key]
        public Guid Id { get; set; }

        [ForeignKey("User")]
        public Guid UserId { get; set; }
        public User User { get; set; } = default!;

        public double TotalPrice { get; set; }
        public DateTime OrderDate { get; set; }
        public bool VipApplied { get; set; }
        public int Status { get; set; }

        [StringLength(100)]
        public string SaleStaff { get; set; } = string.Empty;

        [InverseProperty("Order")]
        public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
        [InverseProperty("Order")]
        public ICollection<Delivery> Deliveries { get; set; } = new List<Delivery>();
        [InverseProperty("Order")]
        public ICollection<Payment> Payments { get; set; } = new List<Payment>();
    }
} 