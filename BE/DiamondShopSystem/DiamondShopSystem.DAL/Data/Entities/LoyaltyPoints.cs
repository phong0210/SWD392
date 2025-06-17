using DiamondShopSystem.DAL.Data.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("LoyaltyPoints")]
public class LoyaltyPoints
{
    [Key]
    [ForeignKey("User")] // Shared PK
    public Guid Id { get; set; }

    [Required]
    [Column("points_earned")]
    public int PointsEarned { get; set; } = 0;

    [Required]
    [Column("points_redeemed")]
    public int PointsRedeemed { get; set; } = 0;

    [Required]
    [Column("last_updated")]
    [DataType(DataType.DateTime)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
    public DateTime LastUpdated { get; set; } = DateTime.UtcNow;

    public virtual User User { get; set; }
}

