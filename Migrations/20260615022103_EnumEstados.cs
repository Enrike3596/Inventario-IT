using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HelpDesk.API.Migrations
{
    /// <inheritdoc />
    public partial class EnumEstados : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Asignacion_ActivoUnico",
                table: "AsignacionesUsuario");

            migrationBuilder.DropColumn(
                name: "Estado",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "Activo",
                table: "AsignacionesUsuario");

            migrationBuilder.DropColumn(
                name: "Disponible",
                table: "Activos");

            migrationBuilder.AddColumn<string>(
                name: "EstadoUsuario",
                table: "Usuarios",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Estado",
                table: "Sedes",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<string>(
                name: "Estado",
                table: "Roles",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<string>(
                name: "Estado",
                table: "Parqueaderos",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<string>(
                name: "TipoMovimiento",
                table: "HistorialActivos",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Estado",
                table: "CategoriasActivo",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AddColumn<string>(
                name: "EstadoAsignacion",
                table: "AsignacionesUsuario",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "EstadoActivo",
                table: "Activos",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Asignacion_ActivoUnico",
                table: "AsignacionesUsuario",
                columns: new[] { "IdActivo", "EstadoAsignacion" },
                unique: true,
                filter: "\"EstadoAsignacion\" = 'Activa'");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Asignacion_ActivoUnico",
                table: "AsignacionesUsuario");

            migrationBuilder.DropColumn(
                name: "EstadoUsuario",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "EstadoAsignacion",
                table: "AsignacionesUsuario");

            migrationBuilder.DropColumn(
                name: "EstadoActivo",
                table: "Activos");

            migrationBuilder.AddColumn<bool>(
                name: "Estado",
                table: "Usuarios",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "Estado",
                table: "Sedes",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<bool>(
                name: "Estado",
                table: "Roles",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<bool>(
                name: "Estado",
                table: "Parqueaderos",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "TipoMovimiento",
                table: "HistorialActivos",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<bool>(
                name: "Estado",
                table: "CategoriasActivo",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20);

            migrationBuilder.AddColumn<bool>(
                name: "Activo",
                table: "AsignacionesUsuario",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Disponible",
                table: "Activos",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Asignacion_ActivoUnico",
                table: "AsignacionesUsuario",
                columns: new[] { "IdActivo", "Activo" },
                unique: true,
                filter: "\"Activo\" = true");
        }
    }
}
