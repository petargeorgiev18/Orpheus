using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Orpheus.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedQuantityPropToWishlistItemEntityMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "WishlistsItems",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "WishlistsItems");
        }
    }
}
