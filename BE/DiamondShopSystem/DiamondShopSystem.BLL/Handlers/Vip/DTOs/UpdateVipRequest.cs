using System;
using System.ComponentModel.DataAnnotations;

namespace DiamondShopSystem.BLL.Handlers.Vip.DTOs
{
    public class UpdateVipRequest
    {
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        [Required]
        [StringLength(50)]
        public string Tier { get; set; }
    }
}
