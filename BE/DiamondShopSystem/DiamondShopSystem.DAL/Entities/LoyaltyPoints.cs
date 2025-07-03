using System;
using System.ComponentModel.DataAnnotations;

namespace DiamondShopSystem.DAL.Entities
{
    public class LoyaltyPoints
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; } = default!;
        public int PointsEarned { get; set; }
        public int PointsRedeemed { get; set; }
        public DateTime LastUpdated { get; set; }
    }
} 