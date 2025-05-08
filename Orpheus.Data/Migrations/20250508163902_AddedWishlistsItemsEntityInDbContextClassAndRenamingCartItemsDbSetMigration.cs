using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Orpheus.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedWishlistsItemsEntityInDbContextClassAndRenamingCartItemsDbSetMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_Carts_CartId",
                table: "CartItems");

            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_Items_ItemId",
                table: "CartItems");

            migrationBuilder.DropForeignKey(
                name: "FK_WishlistItem_Items_ItemId",
                table: "WishlistItem");

            migrationBuilder.DropForeignKey(
                name: "FK_WishlistItem_Wishlists_WishlistId",
                table: "WishlistItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WishlistItem",
                table: "WishlistItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CartItems",
                table: "CartItems");

            migrationBuilder.RenameTable(
                name: "WishlistItem",
                newName: "WishlistsItems");

            migrationBuilder.RenameTable(
                name: "CartItems",
                newName: "CartsItems");

            migrationBuilder.RenameIndex(
                name: "IX_WishlistItem_WishlistId",
                table: "WishlistsItems",
                newName: "IX_WishlistsItems_WishlistId");

            migrationBuilder.RenameIndex(
                name: "IX_WishlistItem_ItemId",
                table: "WishlistsItems",
                newName: "IX_WishlistsItems_ItemId");

            migrationBuilder.RenameIndex(
                name: "IX_CartItems_ItemId",
                table: "CartsItems",
                newName: "IX_CartsItems_ItemId");

            migrationBuilder.RenameIndex(
                name: "IX_CartItems_CartId",
                table: "CartsItems",
                newName: "IX_CartsItems_CartId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WishlistsItems",
                table: "WishlistsItems",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CartsItems",
                table: "CartsItems",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CartsItems_Carts_CartId",
                table: "CartsItems",
                column: "CartId",
                principalTable: "Carts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CartsItems_Items_ItemId",
                table: "CartsItems",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WishlistsItems_Items_ItemId",
                table: "WishlistsItems",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WishlistsItems_Wishlists_WishlistId",
                table: "WishlistsItems",
                column: "WishlistId",
                principalTable: "Wishlists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartsItems_Carts_CartId",
                table: "CartsItems");

            migrationBuilder.DropForeignKey(
                name: "FK_CartsItems_Items_ItemId",
                table: "CartsItems");

            migrationBuilder.DropForeignKey(
                name: "FK_WishlistsItems_Items_ItemId",
                table: "WishlistsItems");

            migrationBuilder.DropForeignKey(
                name: "FK_WishlistsItems_Wishlists_WishlistId",
                table: "WishlistsItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WishlistsItems",
                table: "WishlistsItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CartsItems",
                table: "CartsItems");

            migrationBuilder.RenameTable(
                name: "WishlistsItems",
                newName: "WishlistItem");

            migrationBuilder.RenameTable(
                name: "CartsItems",
                newName: "CartItems");

            migrationBuilder.RenameIndex(
                name: "IX_WishlistsItems_WishlistId",
                table: "WishlistItem",
                newName: "IX_WishlistItem_WishlistId");

            migrationBuilder.RenameIndex(
                name: "IX_WishlistsItems_ItemId",
                table: "WishlistItem",
                newName: "IX_WishlistItem_ItemId");

            migrationBuilder.RenameIndex(
                name: "IX_CartsItems_ItemId",
                table: "CartItems",
                newName: "IX_CartItems_ItemId");

            migrationBuilder.RenameIndex(
                name: "IX_CartsItems_CartId",
                table: "CartItems",
                newName: "IX_CartItems_CartId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WishlistItem",
                table: "WishlistItem",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CartItems",
                table: "CartItems",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_Carts_CartId",
                table: "CartItems",
                column: "CartId",
                principalTable: "Carts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_Items_ItemId",
                table: "CartItems",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WishlistItem_Items_ItemId",
                table: "WishlistItem",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WishlistItem_Wishlists_WishlistId",
                table: "WishlistItem",
                column: "WishlistId",
                principalTable: "Wishlists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
