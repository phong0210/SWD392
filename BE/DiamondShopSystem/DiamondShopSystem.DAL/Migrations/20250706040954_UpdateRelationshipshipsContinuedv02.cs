using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DiamondShopSystem.DAL.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRelationshipshipsContinuedv02 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Deliveries_StaffProfiles_DeliveryStaffId",
                table: "Deliveries");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_OrderDetails_OrderDetailId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_OrderDetailId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Deliveries_DeliveryStaffId",
                table: "Deliveries");

            migrationBuilder.DropColumn(
                name: "OrderDetailId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "DeliveryStaffId",
                table: "Deliveries");

            migrationBuilder.AddColumn<Guid>(
                name: "ProductId",
                table: "OrderDetails",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_ProductId",
                table: "OrderDetails",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_Products_ProductId",
                table: "OrderDetails",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_Products_ProductId",
                table: "OrderDetails");

            migrationBuilder.DropIndex(
                name: "IX_OrderDetails_ProductId",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "OrderDetails");

            migrationBuilder.AddColumn<Guid>(
                name: "OrderDetailId",
                table: "Products",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeliveryStaffId",
                table: "Deliveries",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Products_OrderDetailId",
                table: "Products",
                column: "OrderDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_Deliveries_DeliveryStaffId",
                table: "Deliveries",
                column: "DeliveryStaffId");

            migrationBuilder.AddForeignKey(
                name: "FK_Deliveries_StaffProfiles_DeliveryStaffId",
                table: "Deliveries",
                column: "DeliveryStaffId",
                principalTable: "StaffProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_OrderDetails_OrderDetailId",
                table: "Products",
                column: "OrderDetailId",
                principalTable: "OrderDetails",
                principalColumn: "Id");
        }
    }
}
