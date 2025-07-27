using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Orpheus.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddingMerchAndBrandsForItMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Brands",
                columns: new[] { "Id", "Description", "LogoUrl", "Name" },
                values: new object[,]
                {
                    { new Guid("aaaaaaa1-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), "Official merchandise brand of the German industrial metal band Rammstein.", "/images/brands/rammstein.png", "Rammstein" },
                    { new Guid("bbbbbbb2-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), "Legendary thrash metal band known for aggressive sound and dark imagery.", "/images/brands/slayer.png", "Slayer" },
                    { new Guid("ccccccc3-cccc-cccc-cccc-cccccccccccc"), "Official Metallica merchandise and albums, including 'Master of Puppets'.", "/images/brands/metallica.png", "Metallica" }
                });

            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "Id", "BrandId", "CategoryId", "Description", "IsAvailable", "ItemType", "Name", "Price" },
                values: new object[,]
                {
                    { new Guid("dddddddd-dddd-dddd-dddd-dddddddd0001"), new Guid("aaaaaaa1-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), null, "Official Rammstein band t-shirt with iconic logo. 100% cotton, black.", true, 4, "Rammstein T-Shirt - Logo Edition", 29.99m },
                    { new Guid("dddddddd-dddd-dddd-dddd-dddddddd0002"), new Guid("ccccccc3-cccc-cccc-cccc-cccccccccccc"), null, "Comfortable hoodie featuring 'Master of Puppets' album artwork.", true, 4, "Metallica Hoodie - Master of Puppets", 49.99m },
                    { new Guid("dddddddd-dddd-dddd-dddd-dddddddd0003"), new Guid("bbbbbbb2-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), null, "Adjustable Slayer cap with embroidered eagle logo.", true, 4, "Slayer Cap - Eagle Logo", 19.99m }
                });

            migrationBuilder.InsertData(
                table: "Images",
                columns: new[] { "Id", "IsMain", "ItemId", "Url" },
                values: new object[,]
                {
                    { new Guid("ccccccc1-cccc-cccc-cccc-cccccccccccc"), true, new Guid("dddddddd-dddd-dddd-dddd-dddddddd0001"), "https://www.emp.ie/dw/image/v2/BBQV_PRD/on/demandware.static/-/Sites-master-emp/default/dwa9ad3e79/images/1/0/5/8/105863a.jpg?sfrm=png" },
                    { new Guid("ccccccc2-cccc-cccc-cccc-cccccccccccc"), false, new Guid("dddddddd-dddd-dddd-dddd-dddddddd0001"), "https://www.emp.ie/dw/image/v2/BBQV_PRD/on/demandware.static/-/Sites-master-emp/default/dwe630cd96/images/1/0/5/8/105863b.jpg?sfrm=png" },
                    { new Guid("ccccccc3-cccc-cccc-cccc-cccccccccccc"), true, new Guid("dddddddd-dddd-dddd-dddd-dddddddd0002"), "https://muzikercdn.com/uploads/products/3972/397246/thumb_large_d_gallery_base_dc6883f7.jpg" },
                    { new Guid("ccccccc4-cccc-cccc-cccc-cccccccccccc"), false, new Guid("dddddddd-dddd-dddd-dddd-dddddddd0002"), "https://muzikercdn.com/uploads/product_gallery/3972/397247/main_692dc8f6.jpg" },
                    { new Guid("ccccccc5-cccc-cccc-cccc-cccccccccccc"), true, new Guid("dddddddd-dddd-dddd-dddd-dddddddd0003"), "https://i.ebayimg.com/00/s/MTUzN1gxNjAw/z/RFMAAOSwntxiBu1x/$_57.JPG?set_id=8800005007" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Images",
                keyColumn: "Id",
                keyValue: new Guid("ccccccc1-cccc-cccc-cccc-cccccccccccc"));

            migrationBuilder.DeleteData(
                table: "Images",
                keyColumn: "Id",
                keyValue: new Guid("ccccccc2-cccc-cccc-cccc-cccccccccccc"));

            migrationBuilder.DeleteData(
                table: "Images",
                keyColumn: "Id",
                keyValue: new Guid("ccccccc3-cccc-cccc-cccc-cccccccccccc"));

            migrationBuilder.DeleteData(
                table: "Images",
                keyColumn: "Id",
                keyValue: new Guid("ccccccc4-cccc-cccc-cccc-cccccccccccc"));

            migrationBuilder.DeleteData(
                table: "Images",
                keyColumn: "Id",
                keyValue: new Guid("ccccccc5-cccc-cccc-cccc-cccccccccccc"));

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: new Guid("dddddddd-dddd-dddd-dddd-dddddddd0001"));

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: new Guid("dddddddd-dddd-dddd-dddd-dddddddd0002"));

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: new Guid("dddddddd-dddd-dddd-dddd-dddddddd0003"));

            migrationBuilder.DeleteData(
                table: "Brands",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-aaaa-aaaa-aaaa-aaaaaaaaaaaa"));

            migrationBuilder.DeleteData(
                table: "Brands",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbb2-bbbb-bbbb-bbbb-bbbbbbbbbbbb"));

            migrationBuilder.DeleteData(
                table: "Brands",
                keyColumn: "Id",
                keyValue: new Guid("ccccccc3-cccc-cccc-cccc-cccccccccccc"));
        }
    }
}
