using Microsoft.EntityFrameworkCore;
using DiamondShopSystem.DAL.Entities;

namespace DiamondShopSystem.DAL
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Delivery> Deliveries { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<LoyaltyPoints> LoyaltyPoints { get; set; }
        public DbSet<Vip> Vips { get; set; }
        public DbSet<StaffProfile> StaffProfiles { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Warranty> Warranties { get; set; }
        public DbSet<Promotion> Promotions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("uuid-ossp");

            // Set default value for Guid PKs
            modelBuilder.Entity<Category>()
                .Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()");
            modelBuilder.Entity<Product>()
                .Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()");
            modelBuilder.Entity<OrderDetail>()
                .Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()");
            modelBuilder.Entity<Order>()
                .Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()");
            modelBuilder.Entity<Delivery>()
                .Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()");
            modelBuilder.Entity<Payment>()
                .Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()");
            modelBuilder.Entity<User>()
                .Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()");
            modelBuilder.Entity<LoyaltyPoints>()
                .Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()");
            modelBuilder.Entity<Vip>()
                .Property(e => e.VipId)
                .HasDefaultValueSql("uuid_generate_v4()");
            modelBuilder.Entity<StaffProfile>()
                .Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()");
            modelBuilder.Entity<Role>()
                .Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()");
            modelBuilder.Entity<Warranty>()
                .Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()");
            modelBuilder.Entity<Promotion>()
                .Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()");

            // Category-Product
            modelBuilder.Entity<Category>()
                .HasMany(c => c.Products)
                .WithOne(p => p.Category)
                .HasForeignKey(p => p.CategoryId);

            // Product-OrderDetail
            modelBuilder.Entity<Product>()
                .HasMany(p => p.OrderDetails)
                .WithOne(od => od.Product)
                .HasForeignKey(od => od.ProductId);

            // Product-Warranty
            modelBuilder.Entity<Product>()
                .HasMany(p => p.Warranties)
                .WithOne(w => w.Product)
                .HasForeignKey(w => w.ProductId);

            // Product-Promotion
            modelBuilder.Entity<Product>()
                .HasMany(p => p.Promotions)
                .WithOne(pr => pr.Product)
                .HasForeignKey(pr => pr.AppliesToProductId);

            // Order-OrderDetail
            modelBuilder.Entity<Order>()
                .HasMany(o => o.OrderDetails)
                .WithOne(od => od.Order)
                .HasForeignKey(od => od.OrderId);

            // Order-Delivery
            modelBuilder.Entity<Order>()
                .HasMany(o => o.Deliveries)
                .WithOne(d => d.Order)
                .HasForeignKey(d => d.OrderId);

            // Order-Payment
            modelBuilder.Entity<Order>()
                .HasMany(o => o.Payments)
                .WithOne(p => p.Order)
                .HasForeignKey(p => p.OrderId);

            // User-Order
            modelBuilder.Entity<User>()
                .HasMany(u => u.Orders)
                .WithOne(o => o.User)
                .HasForeignKey(o => o.UserId);

            // User-LoyaltyPoints
            modelBuilder.Entity<User>()
                .HasMany(u => u.LoyaltyPoints)
                .WithOne(lp => lp.User)
                .HasForeignKey(lp => lp.UserId);

            // User-Vip
            modelBuilder.Entity<User>()
                .HasMany(u => u.Vips)
                .WithOne(v => v.User)
                .HasForeignKey(v => v.UserId);

            // StaffProfile-Delivery
            modelBuilder.Entity<StaffProfile>()
                .HasMany(sp => sp.Deliveries)
                .WithOne(d => d.DeliveryStaff)
                .HasForeignKey(d => d.DeliveryStaffId);

            // Role-StaffProfile
            modelBuilder.Entity<Role>()
                .HasMany(r => r.StaffProfiles)
                .WithOne(sp => sp.Role)
                .HasForeignKey(sp => sp.RoleId);
        }
    }
} 