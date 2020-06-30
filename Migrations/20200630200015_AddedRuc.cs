using Microsoft.EntityFrameworkCore.Migrations;

namespace WonosWebApp.Migrations
{
    public partial class AddedRuc : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Ruc",
                table: "AspNetUsers",
                maxLength: 11,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ruc",
                table: "AspNetUsers");
        }
    }
}
