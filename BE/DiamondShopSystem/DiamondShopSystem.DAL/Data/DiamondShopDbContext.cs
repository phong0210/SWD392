using Microsoft.EntityFrameworkCore;

using DiamondShopSystem.BLL.Domain.Entities;
using DiamondShopSystem.BLL.Domain.Enums;
using DiamondShopSystem.BLL.Domain.ValueObjects;
using DiamondShopSystem.DAL.Data.Configurations;

namespace DiamondShopSystem.DAL.Data
{
    public class DiamondShopDbContext : DbContext
    {
        public DiamondShopDbContext(DbContextOptions<DiamondShopDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<StaffProfile> StaffProfiles { get; set; }
        public DbSet<VipStatus> VipStatuses { get; set; }
        public DbSet<LoyaltyPointTransaction> LoyaltyPointTransactions { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Delivery> Deliveries { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Promotion> Promotions { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Warranty> Warranties { get; set; }
        
        public DbSet<CustomerProfile> CustomerProfiles { get; set; }
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new ProductConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
            modelBuilder.ApplyConfiguration(new WarrantyConfiguration());
            modelBuilder.ApplyConfiguration(new PromotionConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
            modelBuilder.ApplyConfiguration(new CustomerProfileConfiguration());
            modelBuilder.ApplyConfiguration(new StaffProfileConfiguration());
            modelBuilder.ApplyConfiguration(new OrderConfiguration());
            modelBuilder.ApplyConfiguration(new OrderDetailConfiguration());
            
            modelBuilder.ApplyConfiguration(new DeliveryConfiguration());
            modelBuilder.ApplyConfiguration(new VipStatusConfiguration());
            modelBuilder.ApplyConfiguration(new LoyaltyPointTransactionConfiguration());

            // Seed Data for Categories
            var category1Id = Guid.Parse("c9d4a0c1-5b0d-4e44-8c8a-0f8b0f8b0f8b");
            var category2Id = Guid.Parse("d1e5b1d2-6c1e-4f55-9d9b-1c9c1c9c1c9c");
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = category1Id, Name = "Rings" },
                new Category { Id = category2Id, Name = "Necklaces" }
            );

            // Seed Data for Products
            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = Guid.Parse("a1b2c3d4-e5f6-7890-1234-567890abcdef"),
                    SKU = "RING001",
                    Name = "Classic Diamond Ring",
                    Description = "A beautiful classic diamond ring.",
                    BasePrice = 1500.00m,
                    CategoryId = category1Id,
                    IsActive = true,
                    Quantity = 10
                },
                new Product
                {
                    Id = Guid.Parse("b2c3d4e5-f6a7-8901-2345-67890abcdef0"),
                    SKU = "NECK001",
                    Name = "Elegant Diamond Necklace",
                    Description = "A stunning diamond necklace for special occasions.",
                    BasePrice = 2500.00m,
                    CategoryId = category2Id,
                    IsActive = true,
                    Quantity = 10
                }
            );

            modelBuilder.Entity<Product>().OwnsOne(p => p.DiamondProperties).HasData(
                new { ProductId = Guid.Parse("a1b2c3d4-e5f6-7890-1234-567890abcdef"), Carat = 1.0m, Color = "D", Clarity = "VS1", Cut = "Excellent" },
                new { ProductId = Guid.Parse("b2c3d4e5-f6a7-8901-2345-67890abcdef0"), Carat = 1.5m, Color = "E", Clarity = "VVS2", Cut = "Very Good" }
            );

            
        }
    }
}