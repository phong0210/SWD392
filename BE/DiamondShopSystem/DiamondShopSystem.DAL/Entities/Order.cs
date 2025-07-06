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

        [Required]
        [ForeignKey("User")]
        public Guid UserId { get; set; }
        public User User { get; set; } = default!;

        [Required]
        public double TotalPrice { get; set; }
        [Required]
        public DateTime OrderDate { get; set; }
        [Required]
        public bool VipApplied { get; set; }
        [Required]
        public int Status { get; set; }

        [Required]
        [StringLength(100)]
        public string SaleStaff { get; set; } = string.Empty;

        [InverseProperty("Order")]
        public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
        public Delivery? Delivery { get; set; }
        [InverseProperty("Order")]
        public ICollection<Payment> Payments { get; set; } = new List<Payment>();
    }
} 