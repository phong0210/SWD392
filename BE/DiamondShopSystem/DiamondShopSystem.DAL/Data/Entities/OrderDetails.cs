using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiamondShopSystem.DAL.Data.Entities
{
    [Table("OrderDetails")]
    public class OrderDetails
    {
        [Key]
        [Required]
        [Column("id")]
        public Guid Id { get; set; }

        [Required]
        [ForeignKey(nameof(OrderId))]
        [Column("order_id")]
        public Guid OrderId { get; set; }

        [Required]
        [ForeignKey(nameof(ProductId))]
        [Column("product_id")]
        public Guid ProductId { get; set; }

        [Required]
        [Column("quantity")]
        public int Quantity { get; set; }

        [Required]
        [Column("unit_price")]
        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = true)]
        public float UnitPrice { get; set; }
        public virtual Order Order { get; set; }
        public virtual Products Product { get; set; }

        public virtual Warranties Warranty { get; set; }
    }
}
