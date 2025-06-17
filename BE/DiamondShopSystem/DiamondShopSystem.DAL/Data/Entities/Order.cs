using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiamondShopSystem.DAL.Data.Entities
{

    public enum Status
    {
        Pending,     // Order is pending and awaiting processing
        Processing,  // Order is currently being processed
        Completed,   // Order has been completed successfully
        Cancelled,   // Order has been cancelled by the user or system
        Refunded     // Order has been refunded to the user
    }

    [Table("Orders")]
    public class Order
    {
        [Key]
        [Required]
        [Column("id")]
        public Guid Id { get; set; }

        [Required]
        [ForeignKey("User")]
        [Column("user_id")]
        public Guid UserId { get; set; }

        [Column("promotion_id")]
        [ForeignKey("Promotion")]
        public Guid? PromotionId { get; set; }

        [Required]
        [Column("total_amount")]
        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = true)]
        public float TotalAmount { get; set; }

        [Required]
        [Column("order_date")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        [Column("vip_applied")]
        public bool VipApplied { get; set; } = false;

        [Column("loyalty_points_used")]
        public int LoyaltyPointsUsed { get; set; } = 0;

        [Column("status")]
        [Required]
        public Status Status { get; set; } = Status.Pending;

        [Column("sale_staff_id")]
        // In Order.cs
        public virtual Promotions? Promotion { get; set; }
        public virtual User User { get; set; }
        public virtual Deliveries? Deliveries { get; set; }
        public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
        public virtual ICollection<OrderDetails> OrderDetails { get; set; } = new List<OrderDetails>();
    }
}