using DiamondShopSystem.BLL.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DiamondShopSystem.DAL.Data.Configurations
{
    public class LoyaltyPointTransactionConfiguration : IEntityTypeConfiguration<LoyaltyPointTransaction>
    {
        public void Configure(EntityTypeBuilder<LoyaltyPointTransaction> builder)
        {
            builder.HasKey(lpt => lpt.Id);
            builder.Property(lpt => lpt.Points).IsRequired();
            builder.Property(lpt => lpt.TransactionType).IsRequired();
            builder.Property(lpt => lpt.Date).IsRequired();

            builder.HasOne(lpt => lpt.Customer)
                   .WithMany(u => u.LoyaltyPointTransactions)
                   .HasForeignKey(lpt => lpt.CustomerId)
                   .OnDelete(DeleteBehavior.Restrict); // Prevent deleting user if loyalty transactions exist
        }
    }
}