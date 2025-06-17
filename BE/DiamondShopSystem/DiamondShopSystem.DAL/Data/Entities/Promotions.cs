using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace DiamondShopSystem.DAL.Data.Entities
{
    [Table("Promotions")]
    public class Promotions
    {
        [Key]
        [Required]
        [Column("id")]
        public Guid Id { get; set; }

        [Required]
        [Column("name")]
        [StringLength(100, ErrorMessage = "Promotion name cannot be longer than 100 characters.")]
        public string Name { get; set; }

        [Required]
        [Column("description")]
        [StringLength(500, ErrorMessage = "Promotion description cannot be longer than 500 characters.")]
        public string Description { get; set; }

        [Required]
        [Column("start_date")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; } = DateTime.UtcNow;
        [Required]
        [Column("end_date")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; } = DateTime.UtcNow.AddDays(30); // Default to 30 days from now

        [Required]
        [Column("discount_type")]
        public string DiscountType { get; set; } // e.g., "Percentage", "FixedAmount"

        [Required]
        [Column("discount_value")]
        [Range(0, double.MaxValue, ErrorMessage = "Discount value must be a positive number.")]
        public float DiscountValue { get; set; } // e.g., 10 for 10% or 100 for $100 off

        [Required]
        [Column("is_active")]
        public bool IsActive { get; set; } = true; // Default to active

        [Required]
        [Column("created_at")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
        public virtual Order Order { get; set; }
        public virtual ICollection<ProductPromotion> ProductPromotions { get; set; } = new List<ProductPromotion>();

    }
