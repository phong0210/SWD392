using DiamondShopSystem.BLL.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DiamondShopSystem.DAL.Data.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.SKU).IsRequired().HasMaxLength(50);
            builder.Property(p => p.Name).IsRequired().HasMaxLength(255);
            builder.Property(p => p.Description).HasMaxLength(1000);
            builder.Property(p => p.BasePrice).HasColumnType("decimal(18,2)");

            // Relationships
            builder.HasOne(p => p.Category)
                   .WithMany(c => c.Products)
                   .HasForeignKey(p => p.CategoryId)
                   .OnDelete(DeleteBehavior.SetNull); // Assuming CategoryId can be null if category is deleted

            builder.HasOne(p => p.Warranty)
                   .WithOne(w => w.Product)
                   .HasForeignKey<Warranty>(w => w.ProductId)
                   .OnDelete(DeleteBehavior.Cascade);

            

            // Owned entity for DiamondProperties
            builder.OwnsOne(p => p.DiamondProperties, dp =>
            {
                dp.Property(d => d.Carat).HasColumnType("decimal(18,2)");
                dp.Property(d => d.Color).HasMaxLength(50);
                dp.Property(d => d.Clarity).HasMaxLength(50);
                dp.Property(d => d.Cut).HasMaxLength(50);
            });
        }
    }
}