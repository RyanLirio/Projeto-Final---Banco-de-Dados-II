using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CineMa.Migrations
{
    /// <inheritdoc />
    public partial class AddFaceEmbedding2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "FaceEmbedding",
                table: "Client",
                type: "varbinary(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FaceEmbedding",
                table: "Client");
        }
    }
}
