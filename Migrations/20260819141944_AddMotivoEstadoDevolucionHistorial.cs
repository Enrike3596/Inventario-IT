using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HelpDesk.API.Migrations
{
    /// <inheritdoc />
    public partial class AddMotivoEstadoDevolucionHistorial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EstadoDevolucion",
                table: "HistorialActivos",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Motivo",
                table: "HistorialActivos",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EstadoDevolucion",
                table: "HistorialActivos");

            migrationBuilder.DropColumn(
                name: "Motivo",
                table: "HistorialActivos");
        }
    }
}
