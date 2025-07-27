using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Orpheus.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddingAccessoriesAndMakingCategoryPropForItemsNullableMirgation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_Categories_CategoryId",
                table: "Items");

            migrationBuilder.AlterColumn<Guid>(
                name: "CategoryId",
                table: "Items",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.InsertData(
                table: "Brands",
                columns: new[] { "Id", "Description", "LogoUrl", "Name" },
                values: new object[,]
                {
                    { new Guid("44444444-4444-4444-4444-444444444444"), "Dunlop is a leading brand for guitar picks, capos, strings, and other accessories.", "https://example.com/dunlop-logo.png", "Dunlop" },
                    { new Guid("55555555-5555-5555-5555-555555555555"), "Boss is a famous brand known for guitar pedals and tuners.", "https://example.com/boss-logo.png", "Boss" }
                });

            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "Id", "BrandId", "CategoryId", "Description", "IsAvailable", "ItemType", "Name", "Price" },
                values: new object[] { new Guid("99999999-9999-9999-9999-999999999999"), new Guid("22222222-2222-2222-2222-222222222222"), null, "High-quality 10ft cable with durable connectors for guitars and other instruments.", true, 3, "Fender Deluxe Instrument Cable", 24.99m });

            migrationBuilder.InsertData(
                table: "Images",
                columns: new[] { "Id", "IsMain", "ItemId", "Url" },
                values: new object[] { new Guid("22222222-2222-2222-2222-222222222222"), true, new Guid("99999999-9999-9999-9999-999999999999"), "https://m.media-amazon.com/images/I/61lELUFnnkL._UF1000,1000_QL80_.jpg" });

            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "Id", "BrandId", "CategoryId", "Description", "IsAvailable", "ItemType", "Name", "Price" },
                values: new object[,]
                {
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-bbbbbbbbbbbb"), new Guid("44444444-4444-4444-4444-444444444444"), null, "Set of 6 precision guitar picks, ideal for fast and articulate playing.", true, 3, "Dunlop Jazz III Picks (6-pack)", 5.99m },
                    { new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"), new Guid("55555555-5555-5555-5555-555555555555"), null, "Compact pedal tuner with bright LED display, suitable for stage or studio use.", true, 3, "Boss TU-3 Chromatic Tuner Pedal", 99.99m }
                });

            migrationBuilder.InsertData(
                table: "Images",
                columns: new[] { "Id", "IsMain", "ItemId", "Url" },
                values: new object[,]
                {
                    { new Guid("33333333-3333-3333-3333-333333333333"), true, new Guid("aaaaaaaa-aaaa-aaaa-aaaa-bbbbbbbbbbbb"), "https://m.media-amazon.com/images/I/61crwsvMPcL._AC_SL1200_.jpg" },
                    { new Guid("44444444-4444-4444-4444-444444444444"), true, new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"), "https://m.media-amazon.com/images/I/61Rx712UAbL._UF894,1000_QL80_.jpg" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Categories_CategoryId",
                table: "Items",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_Categories_CategoryId",
                table: "Items");

            migrationBuilder.DeleteData(
                table: "Images",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"));

            migrationBuilder.DeleteData(
                table: "Images",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"));

            migrationBuilder.DeleteData(
                table: "Images",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"));

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: new Guid("99999999-9999-9999-9999-999999999999"));

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-bbbbbbbbbbbb"));

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"));

            migrationBuilder.DeleteData(
                table: "Brands",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"));

            migrationBuilder.DeleteData(
                table: "Brands",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"));

            migrationBuilder.AlterColumn<Guid>(
                name: "CategoryId",
                table: "Items",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Categories_CategoryId",
                table: "Items",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
