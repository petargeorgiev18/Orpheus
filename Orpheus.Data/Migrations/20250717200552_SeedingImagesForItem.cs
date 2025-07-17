using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Orpheus.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedingImagesForItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Images",
                columns: new[] { "Id", "ItemId", "Url" },
                values: new object[,]
                {
                    { new Guid("55555555-5555-5555-5555-555555555555"), new Guid("44444444-4444-4444-4444-444444444444"), "https://muzikercdn.com/uploads/products/19437/1943741/main_260b9357.jpg" },
                    { new Guid("66666666-6666-6666-6666-666666666666"), new Guid("44444444-4444-4444-4444-444444444444"), "https://muzikercdn.com/uploads/product_gallery/19437/1943742/main_9bea70a7.jpg" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Images",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"));

            migrationBuilder.DeleteData(
                table: "Images",
                keyColumn: "Id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666666"));
        }
    }
}
