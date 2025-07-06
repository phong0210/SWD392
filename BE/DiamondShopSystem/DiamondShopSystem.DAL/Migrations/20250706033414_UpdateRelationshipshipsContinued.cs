using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DiamondShopSystem.DAL.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRelationshipshipsContinued : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Products_ProductId1",
                table: "Categories");

            migrationBuilder.DropForeignKey(
                name: "FK_Deliveries_Users_UserId",
                table: "Deliveries");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_Products_ProductId",
                table: "OrderDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_Promotions_Products_AppliesToProductId",
                table: "Promotions");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Roles_RoleId",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Vips_Users_UserId",
                table: "Vips");

            migrationBuilder.DropIndex(
                name: "IX_Warranties_ProductId",
                table: "Warranties");

            migrationBuilder.DropIndex(
                name: "IX_Vips_UserId",
                table: "Vips");

            migrationBuilder.DropIndex(
                name: "IX_Users_RoleId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Promotions_AppliesToProductId",
                table: "Promotions");

            migrationBuilder.DropIndex(
                name: "IX_OrderDetails_ProductId",
                table: "OrderDetails");

            migrationBuilder.DropIndex(
                name: "IX_LoyaltyPoints_UserId",
                table: "LoyaltyPoints");

            migrationBuilder.DropIndex(
                name: "IX_Deliveries_OrderId",
                table: "Deliveries");

            migrationBuilder.DropIndex(
                name: "IX_Deliveries_UserId",
                table: "Deliveries");

            migrationBuilder.DropIndex(
                name: "IX_Categories_ProductId1",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Deliveries");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "ProductId1",
                table: "Categories");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "Vips",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "StaffProfiles",
                type: "uuid",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "AppliesToProductId",
                table: "Promotions",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<Guid>(
                name: "ProductId",
                table: "Promotions",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "OrderDetailId",
                table: "Products",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Warranties_ProductId",
                table: "Warranties",
                column: "ProductId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Vips_UserId",
                table: "Vips",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StaffProfiles_UserId",
                table: "StaffProfiles",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Promotions_ProductId",
                table: "Promotions",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_OrderDetailId",
                table: "Products",
                column: "OrderDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_LoyaltyPoints_UserId",
                table: "LoyaltyPoints",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Deliveries_OrderId",
                table: "Deliveries",
                column: "OrderId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_OrderDetails_OrderDetailId",
                table: "Products",
                column: "OrderDetailId",
                principalTable: "OrderDetails",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Promotions_Products_ProductId",
                table: "Promotions",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StaffProfiles_Users_UserId",
                table: "StaffProfiles",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Vips_Users_UserId",
                table: "Vips",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_OrderDetails_OrderDetailId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Promotions_Products_ProductId",
                table: "Promotions");

            migrationBuilder.DropForeignKey(
                name: "FK_StaffProfiles_Users_UserId",
                table: "StaffProfiles");

            migrationBuilder.DropForeignKey(
                name: "FK_Vips_Users_UserId",
                table: "Vips");

            migrationBuilder.DropIndex(
                name: "IX_Warranties_ProductId",
                table: "Warranties");

            migrationBuilder.DropIndex(
                name: "IX_Vips_UserId",
                table: "Vips");

            migrationBuilder.DropIndex(
                name: "IX_StaffProfiles_UserId",
                table: "StaffProfiles");

            migrationBuilder.DropIndex(
                name: "IX_Promotions_ProductId",
                table: "Promotions");

            migrationBuilder.DropIndex(
                name: "IX_Products_OrderDetailId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_LoyaltyPoints_UserId",
                table: "LoyaltyPoints");

            migrationBuilder.DropIndex(
                name: "IX_Deliveries_OrderId",
                table: "Deliveries");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "StaffProfiles");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "Promotions");

            migrationBuilder.DropColumn(
                name: "OrderDetailId",
                table: "Products");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "Vips",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "RoleId",
                table: "Users",
                type: "uuid",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "AppliesToProductId",
                table: "Promotions",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProductId",
                table: "OrderDetails",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Deliveries",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ProductId",
                table: "Categories",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ProductId1",
                table: "Categories",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Warranties_ProductId",
                table: "Warranties",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Vips_UserId",
                table: "Vips",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Promotions_AppliesToProductId",
                table: "Promotions",
                column: "AppliesToProductId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_ProductId",
                table: "OrderDetails",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_LoyaltyPoints_UserId",
                table: "LoyaltyPoints",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Deliveries_OrderId",
                table: "Deliveries",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Deliveries_UserId",
                table: "Deliveries",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_ProductId1",
                table: "Categories",
                column: "ProductId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_Products_ProductId1",
                table: "Categories",
                column: "ProductId1",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Deliveries_Users_UserId",
                table: "Deliveries",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_Products_ProductId",
                table: "OrderDetails",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Promotions_Products_AppliesToProductId",
                table: "Promotions",
                column: "AppliesToProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Roles_RoleId",
                table: "Users",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Vips_Users_UserId",
                table: "Vips",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
