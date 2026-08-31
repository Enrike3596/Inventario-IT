using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace HelpDesk.API.Migrations
{
    /// <inheritdoc />
    public partial class RemoveCanalesModuleAndAddTipoCanalEnum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AsignacionesUsuario_Canales_IdCanal",
                table: "AsignacionesUsuario");

            migrationBuilder.DropTable(
                name: "Canales");

            migrationBuilder.DropIndex(
                name: "IX_AsignacionesUsuario_IdCanal",
                table: "AsignacionesUsuario");

            migrationBuilder.DropColumn(
                name: "IdCanal",
                table: "AsignacionesUsuario");

            migrationBuilder.AddColumn<string>(
                name: "Canal",
                table: "AsignacionesUsuario",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Canal",
                table: "AsignacionesUsuario");

            migrationBuilder.AddColumn<int>(
                name: "IdCanal",
                table: "AsignacionesUsuario",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Canales",
                columns: table => new
                {
                    IdCanal = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreadoPor = table.Column<int>(type: "integer", nullable: true),
                    Estado = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    FechaModificacion = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    FechaSolicitud = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ModificadoPor = table.Column<int>(type: "integer", nullable: true),
                    MotivoEdicion = table.Column<string>(type: "text", nullable: true),
                    Nombre = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Canales", x => x.IdCanal);
                    table.ForeignKey(
                        name: "FK_Canales_Usuarios_CreadoPor",
                        column: x => x.CreadoPor,
                        principalTable: "Usuarios",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Canales_Usuarios_ModificadoPor",
                        column: x => x.ModificadoPor,
                        principalTable: "Usuarios",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AsignacionesUsuario_IdCanal",
                table: "AsignacionesUsuario",
                column: "IdCanal");

            migrationBuilder.CreateIndex(
                name: "IX_Canales_CreadoPor",
                table: "Canales",
                column: "CreadoPor");

            migrationBuilder.CreateIndex(
                name: "IX_Canales_ModificadoPor",
                table: "Canales",
                column: "ModificadoPor");

            migrationBuilder.CreateIndex(
                name: "IX_Canales_Nombre",
                table: "Canales",
                column: "Nombre",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AsignacionesUsuario_Canales_IdCanal",
                table: "AsignacionesUsuario",
                column: "IdCanal",
                principalTable: "Canales",
                principalColumn: "IdCanal",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
