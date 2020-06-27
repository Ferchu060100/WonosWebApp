using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WonosWebApp.Migrations
{
    public partial class AddedBonos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bonos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(nullable: true),
                    vnominal = table.Column<double>(nullable: false),
                    vcomercial = table.Column<double>(nullable: false),
                    nroaños = table.Column<int>(nullable: false),
                    frecuencia = table.Column<int>(nullable: false),
                    diasporanio = table.Column<int>(nullable: false),
                    capitalizacion = table.Column<int>(nullable: false),
                    tipoTasa = table.Column<string>(nullable: true),
                    tasaInteres = table.Column<double>(nullable: false),
                    tasaDescuentoCOK = table.Column<double>(nullable: false),
                    impRenta = table.Column<double>(nullable: false),
                    fechaEmision = table.Column<DateTime>(nullable: false),
                    pPrima = table.Column<double>(nullable: false),
                    pEstructura = table.Column<double>(nullable: false),
                    pColoca = table.Column<double>(nullable: false),
                    pFlota = table.Column<double>(nullable: false),
                    pCAVALI = table.Column<double>(nullable: false),
                    TipoMetodo = table.Column<string>(nullable: true),
                    ApplicationUserId = table.Column<int>(nullable: false),
                    ApplicationUserId1 = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bonos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bonos_AspNetUsers_ApplicationUserId1",
                        column: x => x.ApplicationUserId1,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bonos_ApplicationUserId1",
                table: "Bonos",
                column: "ApplicationUserId1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bonos");
        }
    }
}
