using DiamondShopSystem.BLL.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DiamondShopSystem.DAL.Data.Configurations
{
    public class ProductPromotionConfiguration : IEntityTypeConfiguration<ProductPromotion>
    {
        public void Configure(EntityTypeBuilder<ProductPromotion> builder)
        {
            builder.HasKey(pp => new { pp.ProductId, pp.PromotionId });

            builder.HasOne(pp => pp.Product)
                   .WithMany(p => p.ProductPromotions)
                   .HasForeignKey(pp => pp.ProductId);

            builder.HasOne(pp => pp.Promotion)
                   .WithMany(p => p.ProductPromotions)
                   .HasForeignKey(pp => pp.PromotionId);
        }
    }
}