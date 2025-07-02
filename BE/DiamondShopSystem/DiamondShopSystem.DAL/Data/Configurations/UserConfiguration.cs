using DiamondShopSystem.BLL.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DiamondShopSystem.DAL.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);
            builder.Property(u => u.FullName).IsRequired().HasMaxLength(255);
            builder.Property(u => u.Email).IsRequired().HasMaxLength(255);
            builder.HasIndex(u => u.Email).IsUnique();
            builder.Property(u => u.Phone).HasMaxLength(20);
            builder.Property(u => u.PasswordHash).IsRequired().HasMaxLength(255);
            builder.Property(u => u.IsActive).IsRequired();

            // Relationships
            builder.HasOne(u => u.Role)
                   .WithMany()
                   .HasForeignKey(u => u.RoleId)
                   .OnDelete(DeleteBehavior.Restrict); // Prevent deleting a role if users are associated

            builder.HasOne(u => u.CustomerProfile)
                   .WithOne(cp => cp.User)
                   .HasForeignKey<CustomerProfile>(cp => cp.UserId)
                   .OnDelete(DeleteBehavior.Cascade); // Delete customer profile if user is deleted

            builder.HasOne(u => u.StaffProfile)
                   .WithOne(sp => sp.User)
                   .HasForeignKey<StaffProfile>(sp => sp.UserId)
                   .OnDelete(DeleteBehavior.Cascade); // Delete staff profile if user is deleted

            builder.HasMany(u => u.LoyaltyPointTransactions)
                   .WithOne(lpt => lpt.Customer)
                   .HasForeignKey(lpt => lpt.CustomerId)
                   .OnDelete(DeleteBehavior.Restrict); // Prevent deleting user if loyalty transactions exist

            builder.HasMany(u => u.Orders)
                   .WithOne(o => o.Customer)
                   .HasForeignKey(o => o.CustomerId)
                   .OnDelete(DeleteBehavior.Restrict); // Prevent deleting user if orders exist
        }
    }
}