
using System;

namespace DiamondShopSystem.BLL.Handlers.Promotion.DTOs
{
    public class PromotionUpdateDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string DiscountType { get; set; } = string.Empty;
        public string DiscountValue { get; set; } = string.Empty;
        public Guid? AppliesToProductId { get; set; }
    }
}
