using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DiamondShopSystem.DAL.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRelationshipshipsContinuedv04 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Promotions_Products_ProductId",
                table: "Promotions");

            migrationBuilder.DropIndex(
                name: "IX_Promotions_ProductId",
                table: "Promotions");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "Promotions");

            migrationBuilder.CreateIndex(
                name: "IX_Promotions_AppliesToProductId",
                table: "Promotions",
                column: "AppliesToProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Promotions_Products_AppliesToProductId",
                table: "Promotions",
                column: "AppliesToProductId",
                principalTable: "Products",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Promotions_Products_AppliesToProductId",
                table: "Promotions");

            migrationBuilder.DropIndex(
                name: "IX_Promotions_AppliesToProductId",
                table: "Promotions");

            migrationBuilder.AddColumn<Guid>(
                name: "ProductId",
                table: "Promotions",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Promotions_ProductId",
                table: "Promotions",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Promotions_Products_ProductId",
                table: "Promotions",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id");
        }
    }
}
