using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HelpDesk.API.Migrations
{
    /// <inheritdoc />
    public partial class FixHistorialActivoSalidaCascade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HistorialActivos_Salidas_IdSalida",
                table: "HistorialActivos");

            migrationBuilder.AddForeignKey(
                name: "FK_HistorialActivos_Salidas_IdSalida",
                table: "HistorialActivos",
                column: "IdSalida",
                principalTable: "Salidas",
                principalColumn: "IdSalida",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HistorialActivos_Salidas_IdSalida",
                table: "HistorialActivos");

            migrationBuilder.AddForeignKey(
                name: "FK_HistorialActivos_Salidas_IdSalida",
                table: "HistorialActivos",
                column: "IdSalida",
                principalTable: "Salidas",
                principalColumn: "IdSalida",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
