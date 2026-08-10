using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HelpDesk.API.Migrations
{
    /// <inheritdoc />
    public partial class ChangeActaToGroupLevel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActasFirma_AsignacionesUsuario_IdAsignacion",
                table: "ActasFirma");

            migrationBuilder.DropIndex(
                name: "IX_ActasFirma_IdAsignacion",
                table: "ActasFirma");

            migrationBuilder.RenameColumn(
                name: "IdAsignacion",
                table: "ActasFirma",
                newName: "IdDestino");

            migrationBuilder.AddColumn<string>(
                name: "TipoDestino",
                table: "ActasFirma",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_ActasFirma_IdDestino_TipoDestino",
                table: "ActasFirma",
                columns: new[] { "IdDestino", "TipoDestino" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ActasFirma_IdDestino_TipoDestino",
                table: "ActasFirma");

            migrationBuilder.DropColumn(
                name: "TipoDestino",
                table: "ActasFirma");

            migrationBuilder.RenameColumn(
                name: "IdDestino",
                table: "ActasFirma",
                newName: "IdAsignacion");

            migrationBuilder.CreateIndex(
                name: "IX_ActasFirma_IdAsignacion",
                table: "ActasFirma",
                column: "IdAsignacion");

            migrationBuilder.AddForeignKey(
                name: "FK_ActasFirma_AsignacionesUsuario_IdAsignacion",
                table: "ActasFirma",
                column: "IdAsignacion",
                principalTable: "AsignacionesUsuario",
                principalColumn: "IdAsignacion",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
