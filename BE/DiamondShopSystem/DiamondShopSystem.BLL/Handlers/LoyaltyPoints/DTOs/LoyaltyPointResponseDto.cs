
using System;

namespace DiamondShopSystem.BLL.Handlers.LoyaltyPoints.DTOs
{
    public class LoyaltyPointResponseDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public int PointsEarned { get; set; }
        public int PointsRedeemed { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
