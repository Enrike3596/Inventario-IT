using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HelpDesk.API.Migrations
{
    /// <inheritdoc />
    public partial class AddHistorialCamposEstadoYAsignacion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Drop old CHECK constraint before adding new TipoMovimiento values
            migrationBuilder.Sql(
                "ALTER TABLE \"HistorialActivos\" DROP CONSTRAINT \"CK_HistorialActivos_TipoMovimiento\"");

            // Fix any invalid TipoMovimiento values
            var validValues = new[] { "Entrada", "Salida", "Asignacion", "Devolucion", "Reparacion", "Baja" };
            foreach (var invalid in new[] { "Disponible", "Asignado", "EnReparacion", "DadoDeBaja", "Venta" })
            {
                migrationBuilder.Sql(
                    $"UPDATE \"HistorialActivos\" SET \"TipoMovimiento\" = 'Salida' " +
                    $"WHERE \"TipoMovimiento\" = '{invalid}'");
            }

            // Add new CHECK constraint with expanded values
            migrationBuilder.Sql(
                "ALTER TABLE \"HistorialActivos\" ADD CONSTRAINT \"CK_HistorialActivos_TipoMovimiento\" " +
                "CHECK (\"TipoMovimiento\" IN ('Entrada', 'Salida', 'Asignacion', 'Devolucion', 'Reparacion', 'Baja'))");

            migrationBuilder.AddColumn<string>(
                name: "EstadoAnterior",
                table: "HistorialActivos",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EstadoNuevo",
                table: "HistorialActivos",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IdAsignacion",
                table: "HistorialActivos",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Observaciones",
                table: "HistorialActivos",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_HistorialActivos_IdAsignacion",
                table: "HistorialActivos",
                column: "IdAsignacion");

            migrationBuilder.AddForeignKey(
                name: "FK_HistorialActivos_AsignacionesUsuario_IdAsignacion",
                table: "HistorialActivos",
                column: "IdAsignacion",
                principalTable: "AsignacionesUsuario",
                principalColumn: "IdAsignacion",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HistorialActivos_AsignacionesUsuario_IdAsignacion",
                table: "HistorialActivos");

            migrationBuilder.DropIndex(
                name: "IX_HistorialActivos_IdAsignacion",
                table: "HistorialActivos");

            migrationBuilder.DropColumn(
                name: "EstadoAnterior",
                table: "HistorialActivos");

            migrationBuilder.DropColumn(
                name: "EstadoNuevo",
                table: "HistorialActivos");

            migrationBuilder.DropColumn(
                name: "IdAsignacion",
                table: "HistorialActivos");

            migrationBuilder.DropColumn(
                name: "Observaciones",
                table: "HistorialActivos");

            // Restore old CHECK constraint
            migrationBuilder.Sql(
                "ALTER TABLE \"HistorialActivos\" ADD CONSTRAINT \"CK_HistorialActivos_TipoMovimiento\" " +
                "CHECK (\"TipoMovimiento\" IN ('Entrada', 'Salida', 'Asignacion', 'Devolucion'))");
        }
    }
}
