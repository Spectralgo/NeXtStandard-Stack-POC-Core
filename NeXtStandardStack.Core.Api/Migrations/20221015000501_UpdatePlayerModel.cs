using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NeXtStandardStack.Core.Api.Migrations
{
    public partial class UpdatePlayerModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Players",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GamerTag",
                table: "Players",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "Players",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "GamerTag",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "Players");
        }
    }
}
