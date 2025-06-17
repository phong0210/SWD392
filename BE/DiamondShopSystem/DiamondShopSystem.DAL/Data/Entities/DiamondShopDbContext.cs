using Microsoft.EntityFrameworkCore;

namespace DiamondShopSystem.DAL.Data.Entities
{
    public class DiamondShopDbContext : DbContext
    {
        DbSet<User> Users { get; set; }
        DbSet<StaffProfiles> StaffProfiles { get; set; }
        DbSet<VIP> VIPs { get; set; }
        DbSet<LoyaltyPoints> LoyaltyPoints { get; set; }
        DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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
        }

    }
}
