using DiamondShopSystem.BLL.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DiamondShopSystem.DAL.Data.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasKey(r => r.Id);
            builder.Property(r => r.Name).IsRequired().HasMaxLength(50);
            builder.HasIndex(r => r.Name).IsUnique();

            // Seed Data for Roles
            builder.HasData(
                new Role { Id = Guid.Parse("a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a11"), Name = "Customer" },
                new Role { Id = Guid.Parse("a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a12"), Name = "SalesStaff" },
                new Role { Id = Guid.Parse("a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a13"), Name = "StoreManager" },
                new Role { Id = Guid.Parse("a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a14"), Name = "HeadOfficeAdmin" },
                new Role { Id = Guid.Parse("a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a15"), Name = "DeliveryStaff" }
            );
        }
    }
}