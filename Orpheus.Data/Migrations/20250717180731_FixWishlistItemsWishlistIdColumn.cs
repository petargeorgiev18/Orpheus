using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Orpheus.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixWishlistItemsWishlistIdColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_Wishlists_WishlistId",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Items_WishlistId",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "WishlistId",
                table: "Items");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "WishlistId",
                table: "Items",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Items",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                column: "WishlistId",
                value: null);

            migrationBuilder.CreateIndex(
                name: "IX_Items_WishlistId",
                table: "Items",
                column: "WishlistId");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Wishlists_WishlistId",
                table: "Items",
                column: "WishlistId",
                principalTable: "Wishlists",
                principalColumn: "Id");
        }
    }
}
