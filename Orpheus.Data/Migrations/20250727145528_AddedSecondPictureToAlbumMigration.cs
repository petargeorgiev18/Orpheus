using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Orpheus.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedSecondPictureToAlbumMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Images",
                columns: new[] { "Id", "ItemId", "Url" },
                values: new object[] { new Guid("11111111-1111-1111-1111-111111111111"), new Guid("66666666-6666-6666-6666-666666666666"), "https://m.media-amazon.com/images/I/61KduVSVYlL._SX425_.jpg" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Images",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"));
        }
    }
}
