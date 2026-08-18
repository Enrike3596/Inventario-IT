using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HelpDesk.API.Migrations
{
    /// <inheritdoc />
    public partial class AgregarDocumentoRemision : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NombreDocumento",
                table: "Remisiones",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RutaDocumento",
                table: "Remisiones",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NombreDocumento",
                table: "Remisiones");

            migrationBuilder.DropColumn(
                name: "RutaDocumento",
                table: "Remisiones");
        }
    }
}
