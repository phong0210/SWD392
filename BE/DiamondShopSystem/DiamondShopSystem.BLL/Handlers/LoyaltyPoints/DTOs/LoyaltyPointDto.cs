
using System;

namespace DiamondShopSystem.BLL.Handlers.LoyaltyPoints.DTOs
{
    public class LoyaltyPointDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public int PointsEarned { get; set; }
        public int PointsUsed { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
