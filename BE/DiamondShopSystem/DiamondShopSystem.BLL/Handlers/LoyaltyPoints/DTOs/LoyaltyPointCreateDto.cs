using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondShopSystem.BLL.Handlers.LoyaltyPoints.DTOs
{
    public class LoyaltyPointCreateDto
    {
        public Guid UserId { get; set; }
        public int PointsEarned { get; set; }
        public int PointsUsed { get; set; }
    }
}
