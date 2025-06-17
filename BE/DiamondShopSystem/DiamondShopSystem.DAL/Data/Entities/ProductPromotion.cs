

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiamondShopSystem.DAL.Data.Entities
{
    [Table("ProductPromotions")]
    public class ProductPromotion
    {
        [Key, Column(Order = 0)]
        public Guid ProductId { get; set; }
        public Products Product { get; set; }

        [Key, Column(Order = 1)]
        public Guid PromotionId { get; set; }
        public Promotions Promotion { get; set; }
    }
}
