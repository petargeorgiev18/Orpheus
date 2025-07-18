using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Orpheus.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedingCategoriesBrandsAndAlbumsMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Brands",
                columns: new[] { "Id", "Description", "LogoUrl", "Name" },
                values: new object[,]
                {
                    { new Guid("88888888-8888-8888-8888-888888888888"), "Record label for Metallica's albums including 'Master of Puppets'", "/images/brands/elektra_records.png", "Elektra Records" },
                    { new Guid("99999999-9999-9999-9999-999999999999"), "Label for Lady Gaga's 'Born This Way'", "/images/brands/island_records.png", "Island Records" },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), "Label for Pink Floyd's 'The Dark Side of the Moon'", "/images/brands/harvest_records.jpg", "Harvest Records" }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CategoryName" },
                values: new object[,]
                {
                    { new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"), "Albums" },
                    { new Guid("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"), "Merch" },
                    { new Guid("ffffffff-ffff-ffff-ffff-ffffffffffff"), "Accessories" }
                });

            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "Id", "BrandId", "CategoryId", "Description", "IsAvailable", "ItemType", "Name", "Price" },
                values: new object[,]
                {
                    { new Guid("66666666-6666-6666-6666-666666666666"), new Guid("99999999-9999-9999-9999-999999999999"), new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"), "Lady Gaga's 2011 album blending pop, dance, and electronic music, celebrated for its themes of empowerment and individuality.", true, 2, "Born This Way", 19.99m },
                    { new Guid("77777777-7777-7777-7777-777777777777"), new Guid("88888888-8888-8888-8888-888888888888"), new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"), "Metallica's iconic thrash metal album released in 1986.", true, 2, "Master of Puppets", 24.99m },
                    { new Guid("88888888-8888-8888-8888-888888888888"), new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"), "Pink Floyd's 1973 album, a landmark in progressive rock.", true, 2, "The Dark Side of the Moon", 29.99m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("ffffffff-ffff-ffff-ffff-ffffffffffff"));

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666666"));

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: new Guid("77777777-7777-7777-7777-777777777777"));

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: new Guid("88888888-8888-8888-8888-888888888888"));

            migrationBuilder.DeleteData(
                table: "Brands",
                keyColumn: "Id",
                keyValue: new Guid("88888888-8888-8888-8888-888888888888"));

            migrationBuilder.DeleteData(
                table: "Brands",
                keyColumn: "Id",
                keyValue: new Guid("99999999-9999-9999-9999-999999999999"));

            migrationBuilder.DeleteData(
                table: "Brands",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"));
        }
    }
}
