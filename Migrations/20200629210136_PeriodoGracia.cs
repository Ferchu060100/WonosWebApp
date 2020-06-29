using Microsoft.EntityFrameworkCore.Migrations;

namespace WonosWebApp.Migrations
{
    public partial class PeriodoGracia : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Inflacion",
                table: "Bonos",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "PlazoGraciaCant",
                table: "Bonos",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Inflacion",
                table: "Bonos");

            migrationBuilder.DropColumn(
                name: "PlazoGraciaCant",
                table: "Bonos");
        }
    }
}
