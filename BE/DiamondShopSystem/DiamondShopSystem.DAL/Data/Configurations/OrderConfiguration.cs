using DiamondShopSystem.BLL.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DiamondShopSystem.DAL.Data.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(o => o.Id);
            builder.Property(o => o.OrderDate).IsRequired();
            builder.Property(o => o.Status).IsRequired();
            builder.Property(o => o.TotalAmount).HasColumnType("decimal(18,2)");

            builder.HasOne(o => o.Customer)
                   .WithMany(u => u.Orders)
                   .HasForeignKey(o => o.CustomerId)
                   .OnDelete(DeleteBehavior.Restrict); // Prevent deleting user if orders exist

            builder.HasMany(o => o.OrderDetails)
                   .WithOne(od => od.Order)
                   .HasForeignKey(od => od.OrderId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(o => o.Payments)
                   .WithOne(p => p.Order)
                   .HasForeignKey(p => p.OrderId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(o => o.Delivery)
                   .WithOne(d => d.Order)
                   .HasForeignKey<Delivery>(d => d.OrderId)
                   .IsRequired(false) // Delivery can be null initially
                   .OnDelete(DeleteBehavior.SetNull); // Set null if delivery is deleted

            // Owned entity for ShippingAddress
            builder.OwnsOne(o => o.ShippingAddress, sa =>
            {
                sa.Property(ad => ad.Street).HasMaxLength(255);
                sa.Property(ad => ad.City).HasMaxLength(100);
                sa.Property(ad => ad.State).HasMaxLength(100);
                sa.Property(ad => ad.PostalCode).HasMaxLength(20);
                sa.Property(ad => ad.Country).HasMaxLength(100);
            });
        }
    }
}