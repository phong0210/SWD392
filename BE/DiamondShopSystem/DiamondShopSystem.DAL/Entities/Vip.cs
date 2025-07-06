using System;
using System.ComponentModel.DataAnnotations;

namespace DiamondShopSystem.DAL.Entities
{
    public class Vip
    {
        public Guid VipId { get; set; }
        public Guid? UserId { get; set; }
        public User? User { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        [StringLength(50)]
        public string Tier { get; set; } = string.Empty;
    }
} 