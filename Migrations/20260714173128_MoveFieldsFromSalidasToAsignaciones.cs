using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HelpDesk.API.Migrations
{
    /// <inheritdoc />
    public partial class MoveFieldsFromSalidasToAsignaciones : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Salidas_Canales_IdCanal",
                table: "Salidas");

            migrationBuilder.DropForeignKey(
                name: "FK_Salidas_Parqueaderos_IdParqueaderoDestino",
                table: "Salidas");

            migrationBuilder.DropForeignKey(
                name: "FK_Salidas_Usuarios_IdUsuarioEntrega",
                table: "Salidas");

            migrationBuilder.DropIndex(
                name: "IX_Salidas_IdCanal",
                table: "Salidas");

            migrationBuilder.DropIndex(
                name: "IX_Salidas_IdParqueaderoDestino",
                table: "Salidas");

            migrationBuilder.DropIndex(
                name: "IX_Salidas_IdUsuarioEntrega",
                table: "Salidas");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Salida_Destino",
                table: "Salidas");

            migrationBuilder.DropColumn(
                name: "IdCanal",
                table: "Salidas");

            migrationBuilder.DropColumn(
                name: "IdParqueaderoDestino",
                table: "Salidas");

            migrationBuilder.DropColumn(
                name: "IdUsuarioEntrega",
                table: "Salidas");

            migrationBuilder.DropColumn(
                name: "NumeroTicket",
                table: "Salidas");

            migrationBuilder.DropColumn(
                name: "RegistroSalida",
                table: "Salidas");

            migrationBuilder.AlterColumn<int>(
                name: "IdUsuarioEntrega",
                table: "HistorialActivos",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "IdSalida",
                table: "HistorialActivos",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<int>(
                name: "IdCanal",
                table: "AsignacionesUsuario",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IdUsuarioEntrega",
                table: "AsignacionesUsuario",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "NumeroTicket",
                table: "AsignacionesUsuario",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RegistroSalida",
                table: "AsignacionesUsuario",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_AsignacionesUsuario_IdCanal",
                table: "AsignacionesUsuario",
                column: "IdCanal");

            migrationBuilder.CreateIndex(
                name: "IX_AsignacionesUsuario_IdUsuarioEntrega",
                table: "AsignacionesUsuario",
                column: "IdUsuarioEntrega");

            migrationBuilder.AddForeignKey(
                name: "FK_AsignacionesUsuario_Canales_IdCanal",
                table: "AsignacionesUsuario",
                column: "IdCanal",
                principalTable: "Canales",
                principalColumn: "IdCanal",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AsignacionesUsuario_Usuarios_IdUsuarioEntrega",
                table: "AsignacionesUsuario",
                column: "IdUsuarioEntrega",
                principalTable: "Usuarios",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AsignacionesUsuario_Canales_IdCanal",
                table: "AsignacionesUsuario");

            migrationBuilder.DropForeignKey(
                name: "FK_AsignacionesUsuario_Usuarios_IdUsuarioEntrega",
                table: "AsignacionesUsuario");

            migrationBuilder.DropIndex(
                name: "IX_AsignacionesUsuario_IdCanal",
                table: "AsignacionesUsuario");

            migrationBuilder.DropIndex(
                name: "IX_AsignacionesUsuario_IdUsuarioEntrega",
                table: "AsignacionesUsuario");

            migrationBuilder.DropColumn(
                name: "IdCanal",
                table: "AsignacionesUsuario");

            migrationBuilder.DropColumn(
                name: "IdUsuarioEntrega",
                table: "AsignacionesUsuario");

            migrationBuilder.DropColumn(
                name: "NumeroTicket",
                table: "AsignacionesUsuario");

            migrationBuilder.DropColumn(
                name: "RegistroSalida",
                table: "AsignacionesUsuario");

            migrationBuilder.AddColumn<int>(
                name: "IdCanal",
                table: "Salidas",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IdParqueaderoDestino",
                table: "Salidas",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IdUsuarioEntrega",
                table: "Salidas",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "NumeroTicket",
                table: "Salidas",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RegistroSalida",
                table: "Salidas",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "IdUsuarioEntrega",
                table: "HistorialActivos",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "IdSalida",
                table: "HistorialActivos",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Salidas_IdCanal",
                table: "Salidas",
                column: "IdCanal");

            migrationBuilder.CreateIndex(
                name: "IX_Salidas_IdParqueaderoDestino",
                table: "Salidas",
                column: "IdParqueaderoDestino");

            migrationBuilder.CreateIndex(
                name: "IX_Salidas_IdUsuarioEntrega",
                table: "Salidas",
                column: "IdUsuarioEntrega");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Salida_Destino",
                table: "Salidas",
                sql: "\"IdUsuarioDestino\" IS NOT NULL OR \"IdParqueaderoDestino\" IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Salidas_Canales_IdCanal",
                table: "Salidas",
                column: "IdCanal",
                principalTable: "Canales",
                principalColumn: "IdCanal",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Salidas_Parqueaderos_IdParqueaderoDestino",
                table: "Salidas",
                column: "IdParqueaderoDestino",
                principalTable: "Parqueaderos",
                principalColumn: "IdParqueadero",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Salidas_Usuarios_IdUsuarioEntrega",
                table: "Salidas",
                column: "IdUsuarioEntrega",
                principalTable: "Usuarios",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
