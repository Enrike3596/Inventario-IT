using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HelpDesk.API.Migrations
{
    /// <inheritdoc />
    public partial class AgregarFormaEntregaDevolucion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EstadoDevolucion",
                table: "AsignacionesUsuario",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FormaEntregaDevolucion",
                table: "AsignacionesUsuario",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ObservacionDevolucion",
                table: "AsignacionesUsuario",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EstadoDevolucion",
                table: "AsignacionesUsuario");

            migrationBuilder.DropColumn(
                name: "FormaEntregaDevolucion",
                table: "AsignacionesUsuario");

            migrationBuilder.DropColumn(
                name: "ObservacionDevolucion",
                table: "AsignacionesUsuario");
        }
    }
}
