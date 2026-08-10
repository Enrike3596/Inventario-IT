using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace HelpDesk.API.Migrations
{
    /// <inheritdoc />
    public partial class AddActaFirma : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ActasFirma",
                columns: table => new
                {
                    IdActa = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdAsignacion = table.Column<int>(type: "integer", nullable: false),
                    RutaPdf = table.Column<string>(type: "text", nullable: true),
                    Token = table.Column<string>(type: "text", nullable: false),
                    Estado = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    FechaGeneracion = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    FechaEnvio = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    FechaFirma = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    FechaVencimiento = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    NombreFirmante = table.Column<string>(type: "text", nullable: true),
                    DocumentoFirmante = table.Column<string>(type: "text", nullable: true),
                    IpFirma = table.Column<string>(type: "text", nullable: true),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    FechaModificacion = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreadoPor = table.Column<int>(type: "integer", nullable: true),
                    ModificadoPor = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActasFirma", x => x.IdActa);
                    table.ForeignKey(
                        name: "FK_ActasFirma_AsignacionesUsuario_IdAsignacion",
                        column: x => x.IdAsignacion,
                        principalTable: "AsignacionesUsuario",
                        principalColumn: "IdAsignacion",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ActasFirma_Usuarios_CreadoPor",
                        column: x => x.CreadoPor,
                        principalTable: "Usuarios",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_ActasFirma_Usuarios_ModificadoPor",
                        column: x => x.ModificadoPor,
                        principalTable: "Usuarios",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActasFirma_CreadoPor",
                table: "ActasFirma",
                column: "CreadoPor");

            migrationBuilder.CreateIndex(
                name: "IX_ActasFirma_IdAsignacion",
                table: "ActasFirma",
                column: "IdAsignacion");

            migrationBuilder.CreateIndex(
                name: "IX_ActasFirma_ModificadoPor",
                table: "ActasFirma",
                column: "ModificadoPor");

            migrationBuilder.CreateIndex(
                name: "IX_ActasFirma_Token",
                table: "ActasFirma",
                column: "Token",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActasFirma");
        }
    }
}
