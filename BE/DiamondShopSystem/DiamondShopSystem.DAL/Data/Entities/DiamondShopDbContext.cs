using Microsoft.EntityFrameworkCore;

namespace DiamondShopSystem.DAL.Data.Entities
{
    public class DiamondShopDbContext : DbContext
    {
        public DiamondShopDbContext(DbContextOptions<DiamondShopDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<StaffProfiles> StaffProfiles { get; set; }
        public DbSet<VIP> VIPs { get; set; }
        public DbSet<LoyaltyPoints> LoyaltyPoints { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Deliveries> Deliveries { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }
        public DbSet<Products> Products { get; set; }
        public DbSet<Promotions> Promotions { get; set; }
        public DbSet<Categories> Categories { get; set; }
        public DbSet<Warranties> Warranties { get; set; }
        public DbSet<ProductPromotion> ProductPromotions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User 1:1 StaffProfiles
            modelBuilder.Entity<User>()
                .HasOne(u => u.StaffProfiles)
                .WithOne(sp => sp.User)
                .HasForeignKey<StaffProfiles>(sp => sp.Id);

            // StaffProfiles N:1 Role
            modelBuilder.Entity<StaffProfiles>()
                .HasOne(sp => sp.Role)
                .WithMany()
                .HasForeignKey(sp => sp.RoleId);

            // User 1:1 VIP
            modelBuilder.Entity<User>()
                .HasOne(u => u.VIP)
                .WithOne(v => v.User)
                .HasForeignKey<VIP>(v => v.Id);

            // User 1:1 LoyaltyPoints
            modelBuilder.Entity<User>()
                .HasOne(u => u.LoyaltyPoints)
                .WithOne(lp => lp.User)
                .HasForeignKey<LoyaltyPoints>(lp => lp.Id);

            // Order 1:0..1 Deliveries
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Deliveries)
                .WithOne(d => d.Order)
                .HasForeignKey<Deliveries>(d => d.Id);

            // Order 1:N Payments
            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Order)
                .WithMany(o => o.Payments)
                .HasForeignKey(p => p.OrderId);

            // User 1:N Orders
            modelBuilder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Order 1:N OrderDetails
            modelBuilder.Entity<OrderDetails>()
                .HasOne(od => od.Order)
                .WithMany(o => o.OrderDetails)
                .HasForeignKey(od => od.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            // OrderDetails N:1 Products
            modelBuilder.Entity<OrderDetails>()
                .HasOne(od => od.Product)
                .WithMany(p => p.OrderDetails)
                .HasForeignKey(od => od.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            // Order N:1 Promotion (nullable)
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Promotion)
                .WithMany(p => p.Orders)
                .HasForeignKey(o => o.PromotionId)
                .OnDelete(DeleteBehavior.SetNull);

            // M:N Products <-> Promotions via ProductPromotion
            modelBuilder.Entity<ProductPromotion>()
                .HasKey(pp => new { pp.ProductId, pp.PromotionId });

            modelBuilder.Entity<ProductPromotion>()
                .HasOne(pp => pp.Product)
                .WithMany(p => p.ProductPromotions)
                .HasForeignKey(pp => pp.ProductId);

            modelBuilder.Entity<ProductPromotion>()
                .HasOne(pp => pp.Promotion)
                .WithMany(p => p.ProductPromotions)
                .HasForeignKey(pp => pp.PromotionId);

            // Category 1:N Products
            modelBuilder.Entity<Products>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.SetNull);

            // OrderDetail 1:1 Warranty
            modelBuilder.Entity<Warranties>()
                .HasOne(w => w.OrderDetail)
                .WithOne(od => od.Warranty)
                .HasForeignKey<Warranties>(w => w.OrderDetailId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}