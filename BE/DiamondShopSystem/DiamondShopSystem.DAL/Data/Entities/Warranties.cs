using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace DiamondShopSystem.DAL.Data.Entities
{
    [Table("Warranties")]
    public class Warranties
    {
        [Key]
        [Required]
        [Column("id")]
        public Guid Id { get; set; }

        [Required]
        [ForeignKey("OrderDetail")]
        [Column("order_detail_id")]
        public Guid OrderDetailId { get; set; }

        [Column("warranty_start_date")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime? WarrantyStartDate { get; set; } = DateTime.UtcNow;

        [Column("warranty_end_date")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime? WarrantyEndDate { get; set; } = DateTime.UtcNow.AddYears(1); // Default to 1 year warranty

        [Required]
        [Column("details")]
        [StringLength(500, ErrorMessage = "Warranty details cannot be longer than 500 characters.")]
        public string Details { get; set; }

        [Required]
        [Column("is_active")]
        public bool IsActive { get; set; } = false; // Default to active

        public virtual OrderDetails OrderDetail { get; set; }

    }
