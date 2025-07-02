using DiamondShopSystem.BLL.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DiamondShopSystem.DAL.Data.Configurations
{
    public class DeliveryConfiguration : IEntityTypeConfiguration<Delivery>
    {
        public void Configure(EntityTypeBuilder<Delivery> builder)
        {
            builder.HasKey(d => d.Id);
            builder.Property(d => d.Status).IsRequired();

            builder.HasOne(d => d.Order)
                   .WithOne(o => o.Delivery)
                   .HasForeignKey<Delivery>(d => d.OrderId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(d => d.DeliveryStaff)
                   .WithMany()
                   .HasForeignKey(d => d.DeliveryStaffId)
                   .IsRequired(false) // DeliveryStaffId can be null
                   .OnDelete(DeleteBehavior.SetNull); // Set null if DeliveryStaff is deleted
        }
    }
}