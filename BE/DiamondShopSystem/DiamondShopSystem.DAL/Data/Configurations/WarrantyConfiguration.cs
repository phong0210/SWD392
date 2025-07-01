using DiamondShopSystem.BLL.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DiamondShopSystem.DAL.Data.Configurations
{
    public class WarrantyConfiguration : IEntityTypeConfiguration<Warranty>
    {
        public void Configure(EntityTypeBuilder<Warranty> builder)
        {
            builder.HasKey(w => w.Id);
            builder.Property(w => w.Duration).IsRequired();
            builder.Property(w => w.Terms).HasMaxLength(1000);

            builder.HasOne(w => w.Product)
                   .WithOne(p => p.Warranty)
                   .HasForeignKey<Warranty>(w => w.ProductId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}