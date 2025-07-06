using System;
using System.ComponentModel.DataAnnotations;

namespace DiamondShopSystem.DAL.Entities
{
    public class LoyaltyPoints
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid UserId { get; set; }
        public User User { get; set; } = default!;

        [Required]
        public int PointsEarned { get; set; }
        [Required]
        public int PointsRedeemed { get; set; }
        [Required]
        public DateTime LastUpdated { get; set; }
    }
} 