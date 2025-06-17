using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace DiamondShopSystem.DAL.Data.Entities
{
    [Table("Products")]
    public class Products
    {
        [Key]
        [Required]
        [Column("id")]
        public Guid Id { get; set; }

        [Required]
        [Column("name")]
        [StringLength(100, ErrorMessage = "Product name cannot be longer than 100 characters.")]
        public string Name { get; set; }

        [Required]
        [Column("description")]
        [StringLength(500, ErrorMessage = "Product description cannot be longer than 500 characters.")]
        public string Description { get; set; }

        [Required]
        [Column("sku")]
        [StringLength(50, ErrorMessage = "SKU cannot be longer than 50 characters.")]
        public string Sku { get; set; }

        [Required]
        [Column("price")]
        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = true)]
        public float Price { get; set; }

        [Required]
        [Column("stock_quantity")]
        [Range(0, int.MaxValue, ErrorMessage = "Stock quantity cannot be negative.")]
        public int StockQuantity { get; set; } = 0;

        [Required]
        [Column("GIA_certification_number")]
        [StringLength(50, ErrorMessage = "GIA certification number cannot be longer than 50 characters.")]
        public string GiaCertificationNumber { get; set; }

        [Required]
        [Column("is_hidden")]
        public bool IsHidden { get; set; } = false; // Default to not hidden

        [Required]
        [Column("carat")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Carat must be greater than 0.")]
        public float Carat { get; set; }

        [Required]
        [Column("color")]
        [StringLength(20, ErrorMessage = "Color cannot be longer than 20 characters.")]
        public string Color { get; set; }

        [Required]
        [Column("clarity")]
        [StringLength(20, ErrorMessage = "Clarity cannot be longer than 20 characters.")]
        public string Clarity { get; set; }

        [Required]
        [Column("cut")]
        [StringLength(20, ErrorMessage = "Cut cannot be longer than 20 characters.")]
        public string Cut { get; set; }

        [Column("category_id")]
        // Optional Foreign Key
        [ForeignKey("Category")]
        public Guid? CategoryId { get; set; }

        public virtual Categories? Category { get; set; }
        public virtual ICollection<OrderDetails> OrderDetails { get; set; } = new List<OrderDetails>();
        public virtual ICollection<ProductPromotion> ProductPromotions { get; set; } = new List<ProductPromotion>();

    }
}
