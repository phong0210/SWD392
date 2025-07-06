using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DiamondShopSystem.DAL.Migrations
{
    /// <inheritdoc />
    public partial class FixStaffProfileUniqueConstraint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Add unique constraints for 1:1 relationships as per ERD
            migrationBuilder.CreateIndex(
                name: "IX_Deliveries_OrderId_Unique",
                table: "Deliveries",
                column: "OrderId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LoyaltyPoints_UserId_Unique",
                table: "LoyaltyPoints",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StaffProfiles_UserId_Unique",
                table: "StaffProfiles",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Vips_UserId_Unique",
                table: "Vips",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Warranties_ProductId_Unique",
                table: "Warranties",
                column: "ProductId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Remove unique constraints
            migrationBuilder.DropIndex(
                name: "IX_Deliveries_OrderId_Unique",
                table: "Deliveries");

            migrationBuilder.DropIndex(
                name: "IX_LoyaltyPoints_UserId_Unique",
                table: "LoyaltyPoints");

            migrationBuilder.DropIndex(
                name: "IX_StaffProfiles_UserId_Unique",
                table: "StaffProfiles");

            migrationBuilder.DropIndex(
                name: "IX_Vips_UserId_Unique",
                table: "Vips");

            migrationBuilder.DropIndex(
                name: "IX_Warranties_ProductId_Unique",
                table: "Warranties");
        }
    }
}
