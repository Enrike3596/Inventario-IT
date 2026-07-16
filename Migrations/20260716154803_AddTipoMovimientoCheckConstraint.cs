using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HelpDesk.API.Migrations
{
    /// <inheritdoc />
    public partial class AddTipoMovimientoCheckConstraint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Fix any invalid TipoMovimiento values (e.g. incorrectly stored EstadoActivo values)
            // Default invalid records to 'Salida' as they likely belong to a salida process
            var validValues = new[] { "Entrada", "Salida", "Asignacion", "Devolucion" };
            foreach (var invalid in new[] { "Disponible", "Asignado", "EnReparacion", "DadoDeBaja", "Venta" })
            {
                migrationBuilder.Sql(
                    $"UPDATE \"HistorialActivos\" SET \"TipoMovimiento\" = 'Salida' " +
                    $"WHERE \"TipoMovimiento\" = '{invalid}'");
            }

            // Add CHECK constraint to prevent future invalid values
            migrationBuilder.Sql(
                "ALTER TABLE \"HistorialActivos\" ADD CONSTRAINT \"CK_HistorialActivos_TipoMovimiento\" " +
                "CHECK (\"TipoMovimiento\" IN ('Entrada', 'Salida', 'Asignacion', 'Devolucion'))");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                "ALTER TABLE \"HistorialActivos\" DROP CONSTRAINT \"CK_HistorialActivos_TipoMovimiento\"");
        }
    }
}
