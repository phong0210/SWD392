using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace DiamondShopSystem.DAL.Data.Entities
{
    [Table("LoyaltyPoints")]
    public class LoyaltyPoints
    {
        [ForeignKey("User")] 
        [Required]
        [Column("id")]
        public Guid Id { get; set; }

        [Required]
        [Column("points_earned")]
        public int Points_earned { get; set; } = 0;

        [Required]
        [Column("points_redeemed")]
        public int Points_redeemed { get; set; } = 0;

        [Required]
        [Column("last_updated")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;

        public virtual User User { get; set; }
    }
}
