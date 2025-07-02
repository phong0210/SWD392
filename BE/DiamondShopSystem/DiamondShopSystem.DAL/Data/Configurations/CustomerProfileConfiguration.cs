using DiamondShopSystem.BLL.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DiamondShopSystem.DAL.Data.Configurations
{
    public class CustomerProfileConfiguration : IEntityTypeConfiguration<CustomerProfile>
    {
        public void Configure(EntityTypeBuilder<CustomerProfile> builder)
        {
            builder.HasKey(cp => cp.UserId);

            builder.HasOne(cp => cp.User)
                   .WithOne(u => u.CustomerProfile)
                   .HasForeignKey<CustomerProfile>(cp => cp.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(cp => cp.VipStatus)
                   .WithMany(vs => vs.CustomerProfiles)
                   .HasForeignKey(cp => cp.VipStatusId)
                   .IsRequired(false) // VipStatusId can be null
                   .OnDelete(DeleteBehavior.SetNull); // Set null if VipStatus is deleted

            builder.Property(cp => cp.LoyaltyPoints).IsRequired();

            // Owned entity for Address
            builder.OwnsOne(cp => cp.Address, a =>
            {
                a.Property(ad => ad.Street).HasMaxLength(255);
                a.Property(ad => ad.City).HasMaxLength(100);
                a.Property(ad => ad.State).HasMaxLength(100);
                a.Property(ad => ad.PostalCode).HasMaxLength(20);
                a.Property(ad => ad.Country).HasMaxLength(100);
            });
        }
    }
}