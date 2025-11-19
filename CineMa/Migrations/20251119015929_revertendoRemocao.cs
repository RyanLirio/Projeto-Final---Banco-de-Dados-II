using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CineMa.Migrations
{
    /// <inheritdoc />
    public partial class revertendoRemocao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AddColumn<bool>(
            //    name: "NowShowing",
            //    table: "Movie",
            //    type: "bit",
            //    nullable: false,
            //    defaultValue: false);

            //migrationBuilder.AddColumn<bool>(
            //    name: "IsVip",
            //    table: "Chair",
            //    type: "bit",
            //    nullable: false,
            //    defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NowShowing",
                table: "Movie");

            migrationBuilder.DropColumn(
                name: "IsVip",
                table: "Chair");
        }
    }
}
