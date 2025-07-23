using System;

namespace DiamondShopSystem.BLL.Handlers.Vip.DTOs
{
    public class VipDto
    {
        public Guid VipId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Tier { get; set; }
    }
}
