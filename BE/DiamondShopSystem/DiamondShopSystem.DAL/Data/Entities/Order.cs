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
        [Column("id")]
        public Guid UserId { get; set; }

        [Column("promotion_id")]
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
        [Required]
        public Guid? SaleStaffId { get; set; }

        public virtual User User { get; set; }
    }
}
