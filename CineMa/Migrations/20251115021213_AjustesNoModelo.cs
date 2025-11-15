using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CineMa.Migrations
{
    /// <inheritdoc />
    public partial class AjustesNoModelo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FaceEmbedding",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "PersonIdAzure",
                table: "Client");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "FaceEmbedding",
                table: "Client",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PersonIdAzure",
                table: "Client",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
