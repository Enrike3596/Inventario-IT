using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HelpDesk.API.Migrations
{
    /// <inheritdoc />
    public partial class RenameEstadosActivo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("UPDATE \"Activos\" SET \"EstadoActivo\" = 'EnReparacion' WHERE \"EstadoActivo\" = 'EnMantenimiento'");
            migrationBuilder.Sql("UPDATE \"Activos\" SET \"EstadoActivo\" = 'Venta' WHERE \"EstadoActivo\" = 'Vendido'");
            migrationBuilder.Sql("UPDATE \"Salidas\" SET \"EstadoActivo\" = 'EnReparacion' WHERE \"EstadoActivo\" = 'EnMantenimiento'");
            migrationBuilder.Sql("UPDATE \"Salidas\" SET \"EstadoActivo\" = 'Venta' WHERE \"EstadoActivo\" = 'Vendido'");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("UPDATE \"Activos\" SET \"EstadoActivo\" = 'EnMantenimiento' WHERE \"EstadoActivo\" = 'EnReparacion'");
            migrationBuilder.Sql("UPDATE \"Activos\" SET \"EstadoActivo\" = 'Vendido' WHERE \"EstadoActivo\" = 'Venta'");
            migrationBuilder.Sql("UPDATE \"Salidas\" SET \"EstadoActivo\" = 'EnMantenimiento' WHERE \"EstadoActivo\" = 'EnReparacion'");
            migrationBuilder.Sql("UPDATE \"Salidas\" SET \"EstadoActivo\" = 'Vendido' WHERE \"EstadoActivo\" = 'Venta'");
        }
    }
}
