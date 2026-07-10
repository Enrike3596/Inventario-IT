using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HelpDesk.API.Migrations
{
    /// <inheritdoc />
    public partial class AddMotivoEdicion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MotivoEdicion",
                table: "Usuarios",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MotivoEdicion",
                table: "Sedes",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MotivoEdicion",
                table: "Salidas",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MotivoEdicion",
                table: "Roles",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MotivoEdicion",
                table: "Parqueaderos",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MotivoEdicion",
                table: "OrdenesCompra",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MotivoEdicion",
                table: "ItemsOC",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MotivoEdicion",
                table: "CategoriasActivo",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MotivoEdicion",
                table: "Canales",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MotivoEdicion",
                table: "AsignacionesUsuario",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MotivoEdicion",
                table: "Activos",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MotivoEdicion",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "MotivoEdicion",
                table: "Sedes");

            migrationBuilder.DropColumn(
                name: "MotivoEdicion",
                table: "Salidas");

            migrationBuilder.DropColumn(
                name: "MotivoEdicion",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "MotivoEdicion",
                table: "Parqueaderos");

            migrationBuilder.DropColumn(
                name: "MotivoEdicion",
                table: "OrdenesCompra");

            migrationBuilder.DropColumn(
                name: "MotivoEdicion",
                table: "ItemsOC");

            migrationBuilder.DropColumn(
                name: "MotivoEdicion",
                table: "CategoriasActivo");

            migrationBuilder.DropColumn(
                name: "MotivoEdicion",
                table: "Canales");

            migrationBuilder.DropColumn(
                name: "MotivoEdicion",
                table: "AsignacionesUsuario");

            migrationBuilder.DropColumn(
                name: "MotivoEdicion",
                table: "Activos");
        }
    }
}
