using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace HelpDesk.API.Migrations
{
    /// <inheritdoc />
    public partial class EliminarModuloSedes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Usuarios_Sedes_IdSede",
                table: "Usuarios");

            migrationBuilder.DropTable(
                name: "Sedes");

            migrationBuilder.DropIndex(
                name: "IX_Usuarios_IdSede",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "IdSede",
                table: "Usuarios");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdSede",
                table: "Usuarios",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Sedes",
                columns: table => new
                {
                    IdSede = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Ciudad = table.Column<string>(type: "text", nullable: false),
                    CreadoPor = table.Column<int>(type: "integer", nullable: true),
                    Direccion = table.Column<string>(type: "text", nullable: false),
                    Estado = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    FechaModificacion = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    ModificadoPor = table.Column<int>(type: "integer", nullable: true),
                    MotivoEdicion = table.Column<string>(type: "text", nullable: true),
                    Nombre = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sedes", x => x.IdSede);
                    table.ForeignKey(
                        name: "FK_Sedes_Usuarios_CreadoPor",
                        column: x => x.CreadoPor,
                        principalTable: "Usuarios",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Sedes_Usuarios_ModificadoPor",
                        column: x => x.ModificadoPor,
                        principalTable: "Usuarios",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_IdSede",
                table: "Usuarios",
                column: "IdSede");

            migrationBuilder.CreateIndex(
                name: "IX_Sedes_CreadoPor",
                table: "Sedes",
                column: "CreadoPor");

            migrationBuilder.CreateIndex(
                name: "IX_Sedes_ModificadoPor",
                table: "Sedes",
                column: "ModificadoPor");

            migrationBuilder.AddForeignKey(
                name: "FK_Usuarios_Sedes_IdSede",
                table: "Usuarios",
                column: "IdSede",
                principalTable: "Sedes",
                principalColumn: "IdSede",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
