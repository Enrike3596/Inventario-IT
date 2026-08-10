using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HelpDesk.API.Migrations
{
    /// <inheritdoc />
    public partial class SoftDeleteEstados : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Estado",
                table: "Salidas",
                type: "boolean",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "Estado",
                table: "OrdenesCompra",
                type: "boolean",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "Estado",
                table: "ItemsOC",
                type: "boolean",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "Estado",
                table: "DetallesItemOC",
                type: "boolean",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "Estado",
                table: "Canales",
                type: "boolean",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "Activa",
                table: "ActasFirma",
                type: "boolean",
                nullable: false,
                defaultValue: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Estado",
                table: "Salidas");

            migrationBuilder.DropColumn(
                name: "Estado",
                table: "OrdenesCompra");

            migrationBuilder.DropColumn(
                name: "Estado",
                table: "ItemsOC");

            migrationBuilder.DropColumn(
                name: "Estado",
                table: "DetallesItemOC");

            migrationBuilder.DropColumn(
                name: "Estado",
                table: "Canales");

            migrationBuilder.DropColumn(
                name: "Activa",
                table: "ActasFirma");
        }
    }
}
