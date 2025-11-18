using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CineMa.Migrations
{
    /// <inheritdoc />
    public partial class AjustesNovos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Movie",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "NowShowing",
                table: "Movie",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Movie");

            migrationBuilder.DropColumn(
                name: "NowShowing",
                table: "Movie");
        }
    }
}
