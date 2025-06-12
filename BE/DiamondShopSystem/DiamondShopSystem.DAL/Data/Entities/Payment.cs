using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace DiamondShopSystem.DAL.Data.Entities
{
    public enum PaymentMethod
    {
        CreditCard,
        VNPay,
        BankTransfer,
        CashOnDelivery
    } 

    [Table("Payments")]
    public class Payment
    {
        [Key]
        [Required]
        [Column("id")]
        public Guid Id { get; set; }

        [Required]
        [ForeignKey("Order")]
        [Column("order_id")]
        public Guid OrderId { get; set; }

        [Required]
        [Column("amount")]
        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = true)]
        public float Amount { get; set; }

        [Required]
        [Column("payment_date")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;

        [Required]
        [Column("payment_method")]
        public PaymentMethod PaymentMethod { get; set; } = PaymentMethod.VNPay; // Default payment method

        [Required]
        [Column("status")]
        public bool IsSuccessful { get; set; } = true; // Default to successful payment

        public virtual Order Order { get; set; }
    }
}
