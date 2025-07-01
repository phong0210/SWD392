using DiamondShopSystem.BLL.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DiamondShopSystem.DAL.Data.Configurations
{
    public class VipStatusConfiguration : IEntityTypeConfiguration<VipStatus>
    {
        public void Configure(EntityTypeBuilder<VipStatus> builder)
        {
            builder.HasKey(vs => vs.Id);
            builder.Property(vs => vs.Name).IsRequired().HasMaxLength(100);
            builder.Property(vs => vs.RequiredPoints).IsRequired();
            builder.Property(vs => vs.DiscountMultiplier).HasColumnType("decimal(5,2)");

            builder.HasMany(vs => vs.CustomerProfiles)
                   .WithOne(cp => cp.VipStatus)
                   .HasForeignKey(cp => cp.VipStatusId)
                   .OnDelete(DeleteBehavior.SetNull); // Set null if VipStatus is deleted
        }
    }
}