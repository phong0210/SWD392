using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiamondShopSystem.DAL.Data.Entities
{
    [Table("Deliveries")]
    public class Deliveries
    {
        [Key]
        [ForeignKey("Order")]
        [Column("id")]
        public Guid Id { get; set; }

        [Required]
        [Column("delivery_staff_id")]
        public Guid DeliveryStaffId { get; set; }

        [Required]
        [Column("dispatch_time")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime DispatchTime { get; set; } = DateTime.UtcNow;

        [Required]
        [Column("delivery_time")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime DeliveryTime { get; set; } = DateTime.UtcNow;

        [Required]
        [Column("status")]
        public bool IsDelivered { get; set; } = false; // Default to not delivered

        [Required]
        [Column("shipping_address")]
        [StringLength(200, ErrorMessage = "Shipping address cannot be longer than 200 characters.")]
        public string ShippingAddress { get; set; }

        [Required]
        [Column("order_id")]
        public Guid OrderId { get; set; }

        public virtual Order Order { get; set; }
    }
}
