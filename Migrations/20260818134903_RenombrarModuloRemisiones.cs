using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HelpDesk.API.Migrations
{
    /// <inheritdoc />
    public partial class RenombrarModuloRemisiones : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Liberar FK de Activos antes de renombrar tablas/columnas
            migrationBuilder.DropForeignKey(
                name: "FK_Activos_DetallesItemOC_IdDetalleItemOC",
                table: "Activos");

            migrationBuilder.DropForeignKey(
                name: "FK_Activos_ItemsOC_IdItemOC",
                table: "Activos");

            migrationBuilder.DropForeignKey(
                name: "FK_Activos_OrdenesCompra_IdOrden",
                table: "Activos");

            // Columnas de Activos
            migrationBuilder.RenameColumn(
                name: "IdOrden",
                table: "Activos",
                newName: "IdRemision");

            migrationBuilder.RenameColumn(
                name: "IdItemOC",
                table: "Activos",
                newName: "IdItemRemision");

            migrationBuilder.RenameColumn(
                name: "IdDetalleItemOC",
                table: "Activos",
                newName: "IdDetalleItemRemision");

            migrationBuilder.RenameIndex(
                name: "IX_Activos_IdOrden",
                table: "Activos",
                newName: "IX_Activos_IdRemision");

            migrationBuilder.RenameIndex(
                name: "IX_Activos_IdItemOC",
                table: "Activos",
                newName: "IX_Activos_IdItemRemision");

            migrationBuilder.RenameIndex(
                name: "IX_Activos_IdDetalleItemOC",
                table: "Activos",
                newName: "IX_Activos_IdDetalleItemRemision");

            // Tabla Remisiones (antes OrdenesCompra)
            migrationBuilder.RenameTable(
                name: "OrdenesCompra",
                newName: "Remisiones");

            migrationBuilder.RenameColumn(
                name: "IdOrden",
                table: "Remisiones",
                newName: "IdRemision");

            migrationBuilder.RenameColumn(
                name: "NumeroOC",
                table: "Remisiones",
                newName: "NumeroRemision");

            migrationBuilder.RenameIndex(
                name: "IX_OrdenesCompra_NumeroOC",
                table: "Remisiones",
                newName: "IX_Remisiones_NumeroRemision");

            migrationBuilder.RenameIndex(
                name: "IX_OrdenesCompra_CreadoPor",
                table: "Remisiones",
                newName: "IX_Remisiones_CreadoPor");

            migrationBuilder.RenameIndex(
                name: "IX_OrdenesCompra_ModificadoPor",
                table: "Remisiones",
                newName: "IX_Remisiones_ModificadoPor");

            migrationBuilder.Sql("ALTER TABLE \"Remisiones\" RENAME CONSTRAINT \"PK_OrdenesCompra\" TO \"PK_Remisiones\";");
            migrationBuilder.Sql("ALTER TABLE \"Remisiones\" RENAME CONSTRAINT \"FK_OrdenesCompra_Usuarios_CreadoPor\" TO \"FK_Remisiones_Usuarios_CreadoPor\";");
            migrationBuilder.Sql("ALTER TABLE \"Remisiones\" RENAME CONSTRAINT \"FK_OrdenesCompra_Usuarios_ModificadoPor\" TO \"FK_Remisiones_Usuarios_ModificadoPor\";");

            migrationBuilder.DropColumn(
                name: "Total",
                table: "Remisiones");

            migrationBuilder.DropColumn(
                name: "Observaciones",
                table: "Remisiones");

            // Tabla ItemsRemision (antes ItemsOC)
            migrationBuilder.RenameTable(
                name: "ItemsOC",
                newName: "ItemsRemision");

            migrationBuilder.RenameColumn(
                name: "IdItemOC",
                table: "ItemsRemision",
                newName: "IdItemRemision");

            migrationBuilder.RenameColumn(
                name: "IdOrden",
                table: "ItemsRemision",
                newName: "IdRemision");

            migrationBuilder.RenameIndex(
                name: "IX_ItemsOC_IdOrden",
                table: "ItemsRemision",
                newName: "IX_ItemsRemision_IdRemision");

            migrationBuilder.RenameIndex(
                name: "IX_ItemsOC_IdCategoria",
                table: "ItemsRemision",
                newName: "IX_ItemsRemision_IdCategoria");

            migrationBuilder.RenameIndex(
                name: "IX_ItemsOC_CreadoPor",
                table: "ItemsRemision",
                newName: "IX_ItemsRemision_CreadoPor");

            migrationBuilder.RenameIndex(
                name: "IX_ItemsOC_ModificadoPor",
                table: "ItemsRemision",
                newName: "IX_ItemsRemision_ModificadoPor");

            migrationBuilder.Sql("ALTER TABLE \"ItemsRemision\" RENAME CONSTRAINT \"PK_ItemsOC\" TO \"PK_ItemsRemision\";");
            migrationBuilder.Sql("ALTER TABLE \"ItemsRemision\" RENAME CONSTRAINT \"FK_ItemsOC_OrdenesCompra_IdOrden\" TO \"FK_ItemsRemision_Remisiones_IdRemision\";");
            migrationBuilder.Sql("ALTER TABLE \"ItemsRemision\" RENAME CONSTRAINT \"FK_ItemsOC_CategoriasActivo_IdCategoria\" TO \"FK_ItemsRemision_CategoriasActivo_IdCategoria\";");
            migrationBuilder.Sql("ALTER TABLE \"ItemsRemision\" RENAME CONSTRAINT \"FK_ItemsOC_Usuarios_CreadoPor\" TO \"FK_ItemsRemision_Usuarios_CreadoPor\";");
            migrationBuilder.Sql("ALTER TABLE \"ItemsRemision\" RENAME CONSTRAINT \"FK_ItemsOC_Usuarios_ModificadoPor\" TO \"FK_ItemsRemision_Usuarios_ModificadoPor\";");

            migrationBuilder.DropColumn(
                name: "Observaciones",
                table: "ItemsRemision");

            // Tabla DetallesItemRemision (antes DetallesItemOC)
            migrationBuilder.RenameTable(
                name: "DetallesItemOC",
                newName: "DetallesItemRemision");

            migrationBuilder.RenameColumn(
                name: "IdDetalleItemOC",
                table: "DetallesItemRemision",
                newName: "IdDetalleItemRemision");

            migrationBuilder.RenameColumn(
                name: "IdItemOC",
                table: "DetallesItemRemision",
                newName: "IdItemRemision");

            migrationBuilder.RenameIndex(
                name: "IX_DetallesItemOC_IdItemOC",
                table: "DetallesItemRemision",
                newName: "IX_DetallesItemRemision_IdItemRemision");

            migrationBuilder.RenameIndex(
                name: "IX_DetallesItemOC_IdActivo",
                table: "DetallesItemRemision",
                newName: "IX_DetallesItemRemision_IdActivo");

            migrationBuilder.RenameIndex(
                name: "IX_DetallesItemOC_CreadoPor",
                table: "DetallesItemRemision",
                newName: "IX_DetallesItemRemision_CreadoPor");

            migrationBuilder.RenameIndex(
                name: "IX_DetallesItemOC_ModificadoPor",
                table: "DetallesItemRemision",
                newName: "IX_DetallesItemRemision_ModificadoPor");

            migrationBuilder.Sql("ALTER TABLE \"DetallesItemRemision\" RENAME CONSTRAINT \"PK_DetallesItemOC\" TO \"PK_DetallesItemRemision\";");
            migrationBuilder.Sql("ALTER TABLE \"DetallesItemRemision\" RENAME CONSTRAINT \"FK_DetallesItemOC_ItemsOC_IdItemOC\" TO \"FK_DetallesItemRemision_ItemsRemision_IdItemRemision\";");
            migrationBuilder.Sql("ALTER TABLE \"DetallesItemRemision\" RENAME CONSTRAINT \"FK_DetallesItemOC_Activos_IdActivo\" TO \"FK_DetallesItemRemision_Activos_IdActivo\";");
            migrationBuilder.Sql("ALTER TABLE \"DetallesItemRemision\" RENAME CONSTRAINT \"FK_DetallesItemOC_Usuarios_CreadoPor\" TO \"FK_DetallesItemRemision_Usuarios_CreadoPor\";");
            migrationBuilder.Sql("ALTER TABLE \"DetallesItemRemision\" RENAME CONSTRAINT \"FK_DetallesItemOC_Usuarios_ModificadoPor\" TO \"FK_DetallesItemRemision_Usuarios_ModificadoPor\";");

            // Secuencias de identidad
            migrationBuilder.RenameSequence(
                name: "OrdenesCompra_IdOrden_seq",
                newName: "Remisiones_IdRemision_seq");

            migrationBuilder.RenameSequence(
                name: "ItemsOC_IdItemOC_seq",
                newName: "ItemsRemision_IdItemRemision_seq");

            migrationBuilder.RenameSequence(
                name: "DetallesItemOC_IdDetalleItemOC_seq",
                newName: "DetallesItemRemision_IdDetalleItemRemision_seq");

            // Restaurar FK de Activos con los nuevos nombres
            migrationBuilder.AddForeignKey(
                name: "FK_Activos_DetallesItemRemision_IdDetalleItemRemision",
                table: "Activos",
                column: "IdDetalleItemRemision",
                principalTable: "DetallesItemRemision",
                principalColumn: "IdDetalleItemRemision",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Activos_ItemsRemision_IdItemRemision",
                table: "Activos",
                column: "IdItemRemision",
                principalTable: "ItemsRemision",
                principalColumn: "IdItemRemision",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Activos_Remisiones_IdRemision",
                table: "Activos",
                column: "IdRemision",
                principalTable: "Remisiones",
                principalColumn: "IdRemision",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activos_DetallesItemRemision_IdDetalleItemRemision",
                table: "Activos");

            migrationBuilder.DropForeignKey(
                name: "FK_Activos_ItemsRemision_IdItemRemision",
                table: "Activos");

            migrationBuilder.DropForeignKey(
                name: "FK_Activos_Remisiones_IdRemision",
                table: "Activos");

            // Secuencias de identidad
            migrationBuilder.RenameSequence(
                name: "DetallesItemRemision_IdDetalleItemRemision_seq",
                newName: "DetallesItemOC_IdDetalleItemOC_seq");

            migrationBuilder.RenameSequence(
                name: "ItemsRemision_IdItemRemision_seq",
                newName: "ItemsOC_IdItemOC_seq");

            migrationBuilder.RenameSequence(
                name: "Remisiones_IdRemision_seq",
                newName: "OrdenesCompra_IdOrden_seq");

            // Tabla DetallesItemRemision (volver a DetallesItemOC)
            migrationBuilder.Sql("ALTER TABLE \"DetallesItemRemision\" RENAME CONSTRAINT \"FK_DetallesItemRemision_Usuarios_ModificadoPor\" TO \"FK_DetallesItemOC_Usuarios_ModificadoPor\";");
            migrationBuilder.Sql("ALTER TABLE \"DetallesItemRemision\" RENAME CONSTRAINT \"FK_DetallesItemRemision_Usuarios_CreadoPor\" TO \"FK_DetallesItemOC_Usuarios_CreadoPor\";");
            migrationBuilder.Sql("ALTER TABLE \"DetallesItemRemision\" RENAME CONSTRAINT \"FK_DetallesItemRemision_Activos_IdActivo\" TO \"FK_DetallesItemOC_Activos_IdActivo\";");
            migrationBuilder.Sql("ALTER TABLE \"DetallesItemRemision\" RENAME CONSTRAINT \"FK_DetallesItemRemision_ItemsRemision_IdItemRemision\" TO \"FK_DetallesItemOC_ItemsOC_IdItemOC\";");
            migrationBuilder.Sql("ALTER TABLE \"DetallesItemRemision\" RENAME CONSTRAINT \"PK_DetallesItemRemision\" TO \"PK_DetallesItemOC\";");

            migrationBuilder.RenameIndex(
                name: "IX_DetallesItemRemision_ModificadoPor",
                table: "DetallesItemRemision",
                newName: "IX_DetallesItemOC_ModificadoPor");

            migrationBuilder.RenameIndex(
                name: "IX_DetallesItemRemision_CreadoPor",
                table: "DetallesItemRemision",
                newName: "IX_DetallesItemOC_CreadoPor");

            migrationBuilder.RenameIndex(
                name: "IX_DetallesItemRemision_IdActivo",
                table: "DetallesItemRemision",
                newName: "IX_DetallesItemOC_IdActivo");

            migrationBuilder.RenameIndex(
                name: "IX_DetallesItemRemision_IdItemRemision",
                table: "DetallesItemRemision",
                newName: "IX_DetallesItemOC_IdItemOC");

            migrationBuilder.RenameColumn(
                name: "IdItemRemision",
                table: "DetallesItemRemision",
                newName: "IdItemOC");

            migrationBuilder.RenameColumn(
                name: "IdDetalleItemRemision",
                table: "DetallesItemRemision",
                newName: "IdDetalleItemOC");

            migrationBuilder.RenameTable(
                name: "DetallesItemRemision",
                newName: "DetallesItemOC");

            // Tabla ItemsRemision (volver a ItemsOC)
            migrationBuilder.AddColumn<string>(
                name: "Observaciones",
                table: "ItemsRemision",
                type: "text",
                nullable: true);

            migrationBuilder.Sql("ALTER TABLE \"ItemsRemision\" RENAME CONSTRAINT \"FK_ItemsRemision_Usuarios_ModificadoPor\" TO \"FK_ItemsOC_Usuarios_ModificadoPor\";");
            migrationBuilder.Sql("ALTER TABLE \"ItemsRemision\" RENAME CONSTRAINT \"FK_ItemsRemision_Usuarios_CreadoPor\" TO \"FK_ItemsOC_Usuarios_CreadoPor\";");
            migrationBuilder.Sql("ALTER TABLE \"ItemsRemision\" RENAME CONSTRAINT \"FK_ItemsRemision_CategoriasActivo_IdCategoria\" TO \"FK_ItemsOC_CategoriasActivo_IdCategoria\";");
            migrationBuilder.Sql("ALTER TABLE \"ItemsRemision\" RENAME CONSTRAINT \"FK_ItemsRemision_Remisiones_IdRemision\" TO \"FK_ItemsOC_OrdenesCompra_IdOrden\";");
            migrationBuilder.Sql("ALTER TABLE \"ItemsRemision\" RENAME CONSTRAINT \"PK_ItemsRemision\" TO \"PK_ItemsOC\";");

            migrationBuilder.RenameIndex(
                name: "IX_ItemsRemision_ModificadoPor",
                table: "ItemsRemision",
                newName: "IX_ItemsOC_ModificadoPor");

            migrationBuilder.RenameIndex(
                name: "IX_ItemsRemision_CreadoPor",
                table: "ItemsRemision",
                newName: "IX_ItemsOC_CreadoPor");

            migrationBuilder.RenameIndex(
                name: "IX_ItemsRemision_IdCategoria",
                table: "ItemsRemision",
                newName: "IX_ItemsOC_IdCategoria");

            migrationBuilder.RenameIndex(
                name: "IX_ItemsRemision_IdRemision",
                table: "ItemsRemision",
                newName: "IX_ItemsOC_IdOrden");

            migrationBuilder.RenameColumn(
                name: "IdRemision",
                table: "ItemsRemision",
                newName: "IdOrden");

            migrationBuilder.RenameColumn(
                name: "IdItemRemision",
                table: "ItemsRemision",
                newName: "IdItemOC");

            migrationBuilder.RenameTable(
                name: "ItemsRemision",
                newName: "ItemsOC");

            // Tabla Remisiones (volver a OrdenesCompra)
            migrationBuilder.AddColumn<decimal>(
                name: "Total",
                table: "Remisiones",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Observaciones",
                table: "Remisiones",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.Sql("ALTER TABLE \"Remisiones\" RENAME CONSTRAINT \"FK_Remisiones_Usuarios_ModificadoPor\" TO \"FK_OrdenesCompra_Usuarios_ModificadoPor\";");
            migrationBuilder.Sql("ALTER TABLE \"Remisiones\" RENAME CONSTRAINT \"FK_Remisiones_Usuarios_CreadoPor\" TO \"FK_OrdenesCompra_Usuarios_CreadoPor\";");
            migrationBuilder.Sql("ALTER TABLE \"Remisiones\" RENAME CONSTRAINT \"PK_Remisiones\" TO \"PK_OrdenesCompra\";");

            migrationBuilder.RenameIndex(
                name: "IX_Remisiones_ModificadoPor",
                table: "Remisiones",
                newName: "IX_OrdenesCompra_ModificadoPor");

            migrationBuilder.RenameIndex(
                name: "IX_Remisiones_CreadoPor",
                table: "Remisiones",
                newName: "IX_OrdenesCompra_CreadoPor");

            migrationBuilder.RenameIndex(
                name: "IX_Remisiones_NumeroRemision",
                table: "Remisiones",
                newName: "IX_OrdenesCompra_NumeroOC");

            migrationBuilder.RenameColumn(
                name: "NumeroRemision",
                table: "Remisiones",
                newName: "NumeroOC");

            migrationBuilder.RenameColumn(
                name: "IdRemision",
                table: "Remisiones",
                newName: "IdOrden");

            migrationBuilder.RenameTable(
                name: "Remisiones",
                newName: "OrdenesCompra");

            // Columnas de Activos
            migrationBuilder.RenameIndex(
                name: "IX_Activos_IdDetalleItemRemision",
                table: "Activos",
                newName: "IX_Activos_IdDetalleItemOC");

            migrationBuilder.RenameIndex(
                name: "IX_Activos_IdItemRemision",
                table: "Activos",
                newName: "IX_Activos_IdItemOC");

            migrationBuilder.RenameIndex(
                name: "IX_Activos_IdRemision",
                table: "Activos",
                newName: "IX_Activos_IdOrden");

            migrationBuilder.RenameColumn(
                name: "IdDetalleItemRemision",
                table: "Activos",
                newName: "IdDetalleItemOC");

            migrationBuilder.RenameColumn(
                name: "IdItemRemision",
                table: "Activos",
                newName: "IdItemOC");

            migrationBuilder.RenameColumn(
                name: "IdRemision",
                table: "Activos",
                newName: "IdOrden");

            migrationBuilder.AddForeignKey(
                name: "FK_Activos_OrdenesCompra_IdOrden",
                table: "Activos",
                column: "IdOrden",
                principalTable: "OrdenesCompra",
                principalColumn: "IdOrden",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Activos_ItemsOC_IdItemOC",
                table: "Activos",
                column: "IdItemOC",
                principalTable: "ItemsOC",
                principalColumn: "IdItemOC",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Activos_DetallesItemOC_IdDetalleItemOC",
                table: "Activos",
                column: "IdDetalleItemOC",
                principalTable: "DetallesItemOC",
                principalColumn: "IdDetalleItemOC",
                onDelete: ReferentialAction.Restrict);
        }
    }
}