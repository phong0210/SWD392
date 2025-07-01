using DiamondShopSystem.BLL.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DiamondShopSystem.DAL.Data.Configurations
{
    public class StaffProfileConfiguration : IEntityTypeConfiguration<StaffProfile>
    {
        public void Configure(EntityTypeBuilder<StaffProfile> builder)
        {
            builder.HasKey(sp => sp.UserId);

            builder.HasOne(sp => sp.User)
                   .WithOne(u => u.StaffProfile)
                   .HasForeignKey<StaffProfile>(sp => sp.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.Property(sp => sp.HireDate).IsRequired();
            builder.Property(sp => sp.Salary).HasColumnType("decimal(18,2)");
        }
    }
}