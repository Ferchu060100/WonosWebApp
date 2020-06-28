using Microsoft.EntityFrameworkCore.Migrations;

namespace WonosWebApp.Migrations
{
    public partial class ModifiedBono : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bonos_AspNetUsers_ApplicationUserId1",
                table: "Bonos");

            migrationBuilder.DropIndex(
                name: "IX_Bonos_ApplicationUserId1",
                table: "Bonos");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId1",
                table: "Bonos");

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserId",
                table: "Bonos",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Bonos_ApplicationUserId",
                table: "Bonos",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bonos_AspNetUsers_ApplicationUserId",
                table: "Bonos",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bonos_AspNetUsers_ApplicationUserId",
                table: "Bonos");

            migrationBuilder.DropIndex(
                name: "IX_Bonos_ApplicationUserId",
                table: "Bonos");

            migrationBuilder.AlterColumn<int>(
                name: "ApplicationUserId",
                table: "Bonos",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId1",
                table: "Bonos",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Bonos_ApplicationUserId1",
                table: "Bonos",
                column: "ApplicationUserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Bonos_AspNetUsers_ApplicationUserId1",
                table: "Bonos",
                column: "ApplicationUserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
