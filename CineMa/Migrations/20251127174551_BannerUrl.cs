using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CineMa.Migrations
{
    /// <inheritdoc />
    public partial class BannerUrl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UrlBanner",
                table: "Movie",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UrlBanner",
                table: "Movie");
        }
    }
}
