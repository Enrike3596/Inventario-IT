using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HelpDesk.API.Migrations
{
    /// <inheritdoc />
    public partial class AddAuditFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CreadoPor",
                table: "Usuarios",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaModificacion",
                table: "Usuarios",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModificadoPor",
                table: "Usuarios",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreadoPor",
                table: "Sedes",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaCreacion",
                table: "Sedes",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaModificacion",
                table: "Sedes",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModificadoPor",
                table: "Sedes",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreadoPor",
                table: "Salidas",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaCreacion",
                table: "Salidas",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaModificacion",
                table: "Salidas",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModificadoPor",
                table: "Salidas",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreadoPor",
                table: "Roles",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaCreacion",
                table: "Roles",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaModificacion",
                table: "Roles",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModificadoPor",
                table: "Roles",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreadoPor",
                table: "Parqueaderos",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaCreacion",
                table: "Parqueaderos",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaModificacion",
                table: "Parqueaderos",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModificadoPor",
                table: "Parqueaderos",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreadoPor",
                table: "OrdenesCompra",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaCreacion",
                table: "OrdenesCompra",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaModificacion",
                table: "OrdenesCompra",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModificadoPor",
                table: "OrdenesCompra",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreadoPor",
                table: "ItemsOC",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaCreacion",
                table: "ItemsOC",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaModificacion",
                table: "ItemsOC",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModificadoPor",
                table: "ItemsOC",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreadoPor",
                table: "HistorialActivos",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaCreacion",
                table: "HistorialActivos",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaModificacion",
                table: "HistorialActivos",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModificadoPor",
                table: "HistorialActivos",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreadoPor",
                table: "DetallesSalida",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaCreacion",
                table: "DetallesSalida",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaModificacion",
                table: "DetallesSalida",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModificadoPor",
                table: "DetallesSalida",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreadoPor",
                table: "DetallesItemOC",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaCreacion",
                table: "DetallesItemOC",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaModificacion",
                table: "DetallesItemOC",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModificadoPor",
                table: "DetallesItemOC",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreadoPor",
                table: "CategoriasActivo",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaCreacion",
                table: "CategoriasActivo",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaModificacion",
                table: "CategoriasActivo",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModificadoPor",
                table: "CategoriasActivo",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreadoPor",
                table: "Canales",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaCreacion",
                table: "Canales",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaModificacion",
                table: "Canales",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModificadoPor",
                table: "Canales",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreadoPor",
                table: "AsignacionesUsuario",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaCreacion",
                table: "AsignacionesUsuario",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaModificacion",
                table: "AsignacionesUsuario",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModificadoPor",
                table: "AsignacionesUsuario",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreadoPor",
                table: "Activos",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaCreacion",
                table: "Activos",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaModificacion",
                table: "Activos",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModificadoPor",
                table: "Activos",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_CreadoPor",
                table: "Usuarios",
                column: "CreadoPor");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_ModificadoPor",
                table: "Usuarios",
                column: "ModificadoPor");

            migrationBuilder.CreateIndex(
                name: "IX_Sedes_CreadoPor",
                table: "Sedes",
                column: "CreadoPor");

            migrationBuilder.CreateIndex(
                name: "IX_Sedes_ModificadoPor",
                table: "Sedes",
                column: "ModificadoPor");

            migrationBuilder.CreateIndex(
                name: "IX_Salidas_CreadoPor",
                table: "Salidas",
                column: "CreadoPor");

            migrationBuilder.CreateIndex(
                name: "IX_Salidas_ModificadoPor",
                table: "Salidas",
                column: "ModificadoPor");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_CreadoPor",
                table: "Roles",
                column: "CreadoPor");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_ModificadoPor",
                table: "Roles",
                column: "ModificadoPor");

            migrationBuilder.CreateIndex(
                name: "IX_Parqueaderos_CreadoPor",
                table: "Parqueaderos",
                column: "CreadoPor");

            migrationBuilder.CreateIndex(
                name: "IX_Parqueaderos_ModificadoPor",
                table: "Parqueaderos",
                column: "ModificadoPor");

            migrationBuilder.CreateIndex(
                name: "IX_OrdenesCompra_CreadoPor",
                table: "OrdenesCompra",
                column: "CreadoPor");

            migrationBuilder.CreateIndex(
                name: "IX_OrdenesCompra_ModificadoPor",
                table: "OrdenesCompra",
                column: "ModificadoPor");

            migrationBuilder.CreateIndex(
                name: "IX_ItemsOC_CreadoPor",
                table: "ItemsOC",
                column: "CreadoPor");

            migrationBuilder.CreateIndex(
                name: "IX_ItemsOC_ModificadoPor",
                table: "ItemsOC",
                column: "ModificadoPor");

            migrationBuilder.CreateIndex(
                name: "IX_HistorialActivos_CreadoPor",
                table: "HistorialActivos",
                column: "CreadoPor");

            migrationBuilder.CreateIndex(
                name: "IX_HistorialActivos_ModificadoPor",
                table: "HistorialActivos",
                column: "ModificadoPor");

            migrationBuilder.CreateIndex(
                name: "IX_DetallesSalida_CreadoPor",
                table: "DetallesSalida",
                column: "CreadoPor");

            migrationBuilder.CreateIndex(
                name: "IX_DetallesSalida_ModificadoPor",
                table: "DetallesSalida",
                column: "ModificadoPor");

            migrationBuilder.CreateIndex(
                name: "IX_DetallesItemOC_CreadoPor",
                table: "DetallesItemOC",
                column: "CreadoPor");

            migrationBuilder.CreateIndex(
                name: "IX_DetallesItemOC_ModificadoPor",
                table: "DetallesItemOC",
                column: "ModificadoPor");

            migrationBuilder.CreateIndex(
                name: "IX_CategoriasActivo_CreadoPor",
                table: "CategoriasActivo",
                column: "CreadoPor");

            migrationBuilder.CreateIndex(
                name: "IX_CategoriasActivo_ModificadoPor",
                table: "CategoriasActivo",
                column: "ModificadoPor");

            migrationBuilder.CreateIndex(
                name: "IX_Canales_CreadoPor",
                table: "Canales",
                column: "CreadoPor");

            migrationBuilder.CreateIndex(
                name: "IX_Canales_ModificadoPor",
                table: "Canales",
                column: "ModificadoPor");

            migrationBuilder.CreateIndex(
                name: "IX_AsignacionesUsuario_CreadoPor",
                table: "AsignacionesUsuario",
                column: "CreadoPor");

            migrationBuilder.CreateIndex(
                name: "IX_AsignacionesUsuario_ModificadoPor",
                table: "AsignacionesUsuario",
                column: "ModificadoPor");

            migrationBuilder.CreateIndex(
                name: "IX_Activos_CreadoPor",
                table: "Activos",
                column: "CreadoPor");

            migrationBuilder.CreateIndex(
                name: "IX_Activos_ModificadoPor",
                table: "Activos",
                column: "ModificadoPor");

            migrationBuilder.AddForeignKey(
                name: "FK_Activos_Usuarios_CreadoPor",
                table: "Activos",
                column: "CreadoPor",
                principalTable: "Usuarios",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Activos_Usuarios_ModificadoPor",
                table: "Activos",
                column: "ModificadoPor",
                principalTable: "Usuarios",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_AsignacionesUsuario_Usuarios_CreadoPor",
                table: "AsignacionesUsuario",
                column: "CreadoPor",
                principalTable: "Usuarios",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_AsignacionesUsuario_Usuarios_ModificadoPor",
                table: "AsignacionesUsuario",
                column: "ModificadoPor",
                principalTable: "Usuarios",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Canales_Usuarios_CreadoPor",
                table: "Canales",
                column: "CreadoPor",
                principalTable: "Usuarios",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Canales_Usuarios_ModificadoPor",
                table: "Canales",
                column: "ModificadoPor",
                principalTable: "Usuarios",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_CategoriasActivo_Usuarios_CreadoPor",
                table: "CategoriasActivo",
                column: "CreadoPor",
                principalTable: "Usuarios",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_CategoriasActivo_Usuarios_ModificadoPor",
                table: "CategoriasActivo",
                column: "ModificadoPor",
                principalTable: "Usuarios",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_DetallesItemOC_Usuarios_CreadoPor",
                table: "DetallesItemOC",
                column: "CreadoPor",
                principalTable: "Usuarios",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_DetallesItemOC_Usuarios_ModificadoPor",
                table: "DetallesItemOC",
                column: "ModificadoPor",
                principalTable: "Usuarios",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_DetallesSalida_Usuarios_CreadoPor",
                table: "DetallesSalida",
                column: "CreadoPor",
                principalTable: "Usuarios",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_DetallesSalida_Usuarios_ModificadoPor",
                table: "DetallesSalida",
                column: "ModificadoPor",
                principalTable: "Usuarios",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_HistorialActivos_Usuarios_CreadoPor",
                table: "HistorialActivos",
                column: "CreadoPor",
                principalTable: "Usuarios",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_HistorialActivos_Usuarios_ModificadoPor",
                table: "HistorialActivos",
                column: "ModificadoPor",
                principalTable: "Usuarios",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_ItemsOC_Usuarios_CreadoPor",
                table: "ItemsOC",
                column: "CreadoPor",
                principalTable: "Usuarios",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_ItemsOC_Usuarios_ModificadoPor",
                table: "ItemsOC",
                column: "ModificadoPor",
                principalTable: "Usuarios",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_OrdenesCompra_Usuarios_CreadoPor",
                table: "OrdenesCompra",
                column: "CreadoPor",
                principalTable: "Usuarios",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_OrdenesCompra_Usuarios_ModificadoPor",
                table: "OrdenesCompra",
                column: "ModificadoPor",
                principalTable: "Usuarios",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Parqueaderos_Usuarios_CreadoPor",
                table: "Parqueaderos",
                column: "CreadoPor",
                principalTable: "Usuarios",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Parqueaderos_Usuarios_ModificadoPor",
                table: "Parqueaderos",
                column: "ModificadoPor",
                principalTable: "Usuarios",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Roles_Usuarios_CreadoPor",
                table: "Roles",
                column: "CreadoPor",
                principalTable: "Usuarios",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Roles_Usuarios_ModificadoPor",
                table: "Roles",
                column: "ModificadoPor",
                principalTable: "Usuarios",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Salidas_Usuarios_CreadoPor",
                table: "Salidas",
                column: "CreadoPor",
                principalTable: "Usuarios",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Salidas_Usuarios_ModificadoPor",
                table: "Salidas",
                column: "ModificadoPor",
                principalTable: "Usuarios",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Sedes_Usuarios_CreadoPor",
                table: "Sedes",
                column: "CreadoPor",
                principalTable: "Usuarios",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Sedes_Usuarios_ModificadoPor",
                table: "Sedes",
                column: "ModificadoPor",
                principalTable: "Usuarios",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Usuarios_Usuarios_CreadoPor",
                table: "Usuarios",
                column: "CreadoPor",
                principalTable: "Usuarios",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Usuarios_Usuarios_ModificadoPor",
                table: "Usuarios",
                column: "ModificadoPor",
                principalTable: "Usuarios",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activos_Usuarios_CreadoPor",
                table: "Activos");

            migrationBuilder.DropForeignKey(
                name: "FK_Activos_Usuarios_ModificadoPor",
                table: "Activos");

            migrationBuilder.DropForeignKey(
                name: "FK_AsignacionesUsuario_Usuarios_CreadoPor",
                table: "AsignacionesUsuario");

            migrationBuilder.DropForeignKey(
                name: "FK_AsignacionesUsuario_Usuarios_ModificadoPor",
                table: "AsignacionesUsuario");

            migrationBuilder.DropForeignKey(
                name: "FK_Canales_Usuarios_CreadoPor",
                table: "Canales");

            migrationBuilder.DropForeignKey(
                name: "FK_Canales_Usuarios_ModificadoPor",
                table: "Canales");

            migrationBuilder.DropForeignKey(
                name: "FK_CategoriasActivo_Usuarios_CreadoPor",
                table: "CategoriasActivo");

            migrationBuilder.DropForeignKey(
                name: "FK_CategoriasActivo_Usuarios_ModificadoPor",
                table: "CategoriasActivo");

            migrationBuilder.DropForeignKey(
                name: "FK_DetallesItemOC_Usuarios_CreadoPor",
                table: "DetallesItemOC");

            migrationBuilder.DropForeignKey(
                name: "FK_DetallesItemOC_Usuarios_ModificadoPor",
                table: "DetallesItemOC");

            migrationBuilder.DropForeignKey(
                name: "FK_DetallesSalida_Usuarios_CreadoPor",
                table: "DetallesSalida");

            migrationBuilder.DropForeignKey(
                name: "FK_DetallesSalida_Usuarios_ModificadoPor",
                table: "DetallesSalida");

            migrationBuilder.DropForeignKey(
                name: "FK_HistorialActivos_Usuarios_CreadoPor",
                table: "HistorialActivos");

            migrationBuilder.DropForeignKey(
                name: "FK_HistorialActivos_Usuarios_ModificadoPor",
                table: "HistorialActivos");

            migrationBuilder.DropForeignKey(
                name: "FK_ItemsOC_Usuarios_CreadoPor",
                table: "ItemsOC");

            migrationBuilder.DropForeignKey(
                name: "FK_ItemsOC_Usuarios_ModificadoPor",
                table: "ItemsOC");

            migrationBuilder.DropForeignKey(
                name: "FK_OrdenesCompra_Usuarios_CreadoPor",
                table: "OrdenesCompra");

            migrationBuilder.DropForeignKey(
                name: "FK_OrdenesCompra_Usuarios_ModificadoPor",
                table: "OrdenesCompra");

            migrationBuilder.DropForeignKey(
                name: "FK_Parqueaderos_Usuarios_CreadoPor",
                table: "Parqueaderos");

            migrationBuilder.DropForeignKey(
                name: "FK_Parqueaderos_Usuarios_ModificadoPor",
                table: "Parqueaderos");

            migrationBuilder.DropForeignKey(
                name: "FK_Roles_Usuarios_CreadoPor",
                table: "Roles");

            migrationBuilder.DropForeignKey(
                name: "FK_Roles_Usuarios_ModificadoPor",
                table: "Roles");

            migrationBuilder.DropForeignKey(
                name: "FK_Salidas_Usuarios_CreadoPor",
                table: "Salidas");

            migrationBuilder.DropForeignKey(
                name: "FK_Salidas_Usuarios_ModificadoPor",
                table: "Salidas");

            migrationBuilder.DropForeignKey(
                name: "FK_Sedes_Usuarios_CreadoPor",
                table: "Sedes");

            migrationBuilder.DropForeignKey(
                name: "FK_Sedes_Usuarios_ModificadoPor",
                table: "Sedes");

            migrationBuilder.DropForeignKey(
                name: "FK_Usuarios_Usuarios_CreadoPor",
                table: "Usuarios");

            migrationBuilder.DropForeignKey(
                name: "FK_Usuarios_Usuarios_ModificadoPor",
                table: "Usuarios");

            migrationBuilder.DropIndex(
                name: "IX_Usuarios_CreadoPor",
                table: "Usuarios");

            migrationBuilder.DropIndex(
                name: "IX_Usuarios_ModificadoPor",
                table: "Usuarios");

            migrationBuilder.DropIndex(
                name: "IX_Sedes_CreadoPor",
                table: "Sedes");

            migrationBuilder.DropIndex(
                name: "IX_Sedes_ModificadoPor",
                table: "Sedes");

            migrationBuilder.DropIndex(
                name: "IX_Salidas_CreadoPor",
                table: "Salidas");

            migrationBuilder.DropIndex(
                name: "IX_Salidas_ModificadoPor",
                table: "Salidas");

            migrationBuilder.DropIndex(
                name: "IX_Roles_CreadoPor",
                table: "Roles");

            migrationBuilder.DropIndex(
                name: "IX_Roles_ModificadoPor",
                table: "Roles");

            migrationBuilder.DropIndex(
                name: "IX_Parqueaderos_CreadoPor",
                table: "Parqueaderos");

            migrationBuilder.DropIndex(
                name: "IX_Parqueaderos_ModificadoPor",
                table: "Parqueaderos");

            migrationBuilder.DropIndex(
                name: "IX_OrdenesCompra_CreadoPor",
                table: "OrdenesCompra");

            migrationBuilder.DropIndex(
                name: "IX_OrdenesCompra_ModificadoPor",
                table: "OrdenesCompra");

            migrationBuilder.DropIndex(
                name: "IX_ItemsOC_CreadoPor",
                table: "ItemsOC");

            migrationBuilder.DropIndex(
                name: "IX_ItemsOC_ModificadoPor",
                table: "ItemsOC");

            migrationBuilder.DropIndex(
                name: "IX_HistorialActivos_CreadoPor",
                table: "HistorialActivos");

            migrationBuilder.DropIndex(
                name: "IX_HistorialActivos_ModificadoPor",
                table: "HistorialActivos");

            migrationBuilder.DropIndex(
                name: "IX_DetallesSalida_CreadoPor",
                table: "DetallesSalida");

            migrationBuilder.DropIndex(
                name: "IX_DetallesSalida_ModificadoPor",
                table: "DetallesSalida");

            migrationBuilder.DropIndex(
                name: "IX_DetallesItemOC_CreadoPor",
                table: "DetallesItemOC");

            migrationBuilder.DropIndex(
                name: "IX_DetallesItemOC_ModificadoPor",
                table: "DetallesItemOC");

            migrationBuilder.DropIndex(
                name: "IX_CategoriasActivo_CreadoPor",
                table: "CategoriasActivo");

            migrationBuilder.DropIndex(
                name: "IX_CategoriasActivo_ModificadoPor",
                table: "CategoriasActivo");

            migrationBuilder.DropIndex(
                name: "IX_Canales_CreadoPor",
                table: "Canales");

            migrationBuilder.DropIndex(
                name: "IX_Canales_ModificadoPor",
                table: "Canales");

            migrationBuilder.DropIndex(
                name: "IX_AsignacionesUsuario_CreadoPor",
                table: "AsignacionesUsuario");

            migrationBuilder.DropIndex(
                name: "IX_AsignacionesUsuario_ModificadoPor",
                table: "AsignacionesUsuario");

            migrationBuilder.DropIndex(
                name: "IX_Activos_CreadoPor",
                table: "Activos");

            migrationBuilder.DropIndex(
                name: "IX_Activos_ModificadoPor",
                table: "Activos");

            migrationBuilder.DropColumn(
                name: "CreadoPor",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "FechaModificacion",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "ModificadoPor",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "CreadoPor",
                table: "Sedes");

            migrationBuilder.DropColumn(
                name: "FechaCreacion",
                table: "Sedes");

            migrationBuilder.DropColumn(
                name: "FechaModificacion",
                table: "Sedes");

            migrationBuilder.DropColumn(
                name: "ModificadoPor",
                table: "Sedes");

            migrationBuilder.DropColumn(
                name: "CreadoPor",
                table: "Salidas");

            migrationBuilder.DropColumn(
                name: "FechaCreacion",
                table: "Salidas");

            migrationBuilder.DropColumn(
                name: "FechaModificacion",
                table: "Salidas");

            migrationBuilder.DropColumn(
                name: "ModificadoPor",
                table: "Salidas");

            migrationBuilder.DropColumn(
                name: "CreadoPor",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "FechaCreacion",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "FechaModificacion",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "ModificadoPor",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "CreadoPor",
                table: "Parqueaderos");

            migrationBuilder.DropColumn(
                name: "FechaCreacion",
                table: "Parqueaderos");

            migrationBuilder.DropColumn(
                name: "FechaModificacion",
                table: "Parqueaderos");

            migrationBuilder.DropColumn(
                name: "ModificadoPor",
                table: "Parqueaderos");

            migrationBuilder.DropColumn(
                name: "CreadoPor",
                table: "OrdenesCompra");

            migrationBuilder.DropColumn(
                name: "FechaCreacion",
                table: "OrdenesCompra");

            migrationBuilder.DropColumn(
                name: "FechaModificacion",
                table: "OrdenesCompra");

            migrationBuilder.DropColumn(
                name: "ModificadoPor",
                table: "OrdenesCompra");

            migrationBuilder.DropColumn(
                name: "CreadoPor",
                table: "ItemsOC");

            migrationBuilder.DropColumn(
                name: "FechaCreacion",
                table: "ItemsOC");

            migrationBuilder.DropColumn(
                name: "FechaModificacion",
                table: "ItemsOC");

            migrationBuilder.DropColumn(
                name: "ModificadoPor",
                table: "ItemsOC");

            migrationBuilder.DropColumn(
                name: "CreadoPor",
                table: "HistorialActivos");

            migrationBuilder.DropColumn(
                name: "FechaCreacion",
                table: "HistorialActivos");

            migrationBuilder.DropColumn(
                name: "FechaModificacion",
                table: "HistorialActivos");

            migrationBuilder.DropColumn(
                name: "ModificadoPor",
                table: "HistorialActivos");

            migrationBuilder.DropColumn(
                name: "CreadoPor",
                table: "DetallesSalida");

            migrationBuilder.DropColumn(
                name: "FechaCreacion",
                table: "DetallesSalida");

            migrationBuilder.DropColumn(
                name: "FechaModificacion",
                table: "DetallesSalida");

            migrationBuilder.DropColumn(
                name: "ModificadoPor",
                table: "DetallesSalida");

            migrationBuilder.DropColumn(
                name: "CreadoPor",
                table: "DetallesItemOC");

            migrationBuilder.DropColumn(
                name: "FechaCreacion",
                table: "DetallesItemOC");

            migrationBuilder.DropColumn(
                name: "FechaModificacion",
                table: "DetallesItemOC");

            migrationBuilder.DropColumn(
                name: "ModificadoPor",
                table: "DetallesItemOC");

            migrationBuilder.DropColumn(
                name: "CreadoPor",
                table: "CategoriasActivo");

            migrationBuilder.DropColumn(
                name: "FechaCreacion",
                table: "CategoriasActivo");

            migrationBuilder.DropColumn(
                name: "FechaModificacion",
                table: "CategoriasActivo");

            migrationBuilder.DropColumn(
                name: "ModificadoPor",
                table: "CategoriasActivo");

            migrationBuilder.DropColumn(
                name: "CreadoPor",
                table: "Canales");

            migrationBuilder.DropColumn(
                name: "FechaCreacion",
                table: "Canales");

            migrationBuilder.DropColumn(
                name: "FechaModificacion",
                table: "Canales");

            migrationBuilder.DropColumn(
                name: "ModificadoPor",
                table: "Canales");

            migrationBuilder.DropColumn(
                name: "CreadoPor",
                table: "AsignacionesUsuario");

            migrationBuilder.DropColumn(
                name: "FechaCreacion",
                table: "AsignacionesUsuario");

            migrationBuilder.DropColumn(
                name: "FechaModificacion",
                table: "AsignacionesUsuario");

            migrationBuilder.DropColumn(
                name: "ModificadoPor",
                table: "AsignacionesUsuario");

            migrationBuilder.DropColumn(
                name: "CreadoPor",
                table: "Activos");

            migrationBuilder.DropColumn(
                name: "FechaCreacion",
                table: "Activos");

            migrationBuilder.DropColumn(
                name: "FechaModificacion",
                table: "Activos");

            migrationBuilder.DropColumn(
                name: "ModificadoPor",
                table: "Activos");
        }
    }
}
