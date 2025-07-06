using System;
using System.ComponentModel.DataAnnotations;

namespace DiamondShopSystem.DAL.Entities
{
    public class Vip
    {
        [Key]
        public Guid VipId { get; set; }
        public Guid? UserId { get; set; }
        public User? User { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        [Required]
        [StringLength(50)]
        public string Tier { get; set; } = string.Empty;
    }
} 