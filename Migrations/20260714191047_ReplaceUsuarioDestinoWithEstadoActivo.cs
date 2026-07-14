using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HelpDesk.API.Migrations
{
    /// <inheritdoc />
    public partial class ReplaceUsuarioDestinoWithEstadoActivo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Salidas_Usuarios_IdUsuarioDestino",
                table: "Salidas");

            migrationBuilder.DropIndex(
                name: "IX_Salidas_IdUsuarioDestino",
                table: "Salidas");

            migrationBuilder.DropColumn(
                name: "IdUsuarioDestino",
                table: "Salidas");

            migrationBuilder.AddColumn<string>(
                name: "EstadoActivo",
                table: "Salidas",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EstadoActivo",
                table: "Salidas");

            migrationBuilder.AddColumn<int>(
                name: "IdUsuarioDestino",
                table: "Salidas",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Salidas_IdUsuarioDestino",
                table: "Salidas",
                column: "IdUsuarioDestino");

            migrationBuilder.AddForeignKey(
                name: "FK_Salidas_Usuarios_IdUsuarioDestino",
                table: "Salidas",
                column: "IdUsuarioDestino",
                principalTable: "Usuarios",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
