using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace HelpDesk.API.Migrations
{
    /// <inheritdoc />
    public partial class AddAreaModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdArea",
                table: "Usuarios",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Areas",
                columns: table => new
                {
                    IdArea = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NombreArea = table.Column<string>(type: "text", nullable: false),
                    Estado = table.Column<bool>(type: "boolean", nullable: false),
                    MotivoEdicion = table.Column<string>(type: "text", nullable: true),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    FechaModificacion = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreadoPor = table.Column<int>(type: "integer", nullable: true),
                    ModificadoPor = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Areas", x => x.IdArea);
                    table.ForeignKey(
                        name: "FK_Areas_Usuarios_CreadoPor",
                        column: x => x.CreadoPor,
                        principalTable: "Usuarios",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Areas_Usuarios_ModificadoPor",
                        column: x => x.ModificadoPor,
                        principalTable: "Usuarios",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_IdArea",
                table: "Usuarios",
                column: "IdArea");

            migrationBuilder.CreateIndex(
                name: "IX_Areas_CreadoPor",
                table: "Areas",
                column: "CreadoPor");

            migrationBuilder.CreateIndex(
                name: "IX_Areas_ModificadoPor",
                table: "Areas",
                column: "ModificadoPor");

            migrationBuilder.CreateIndex(
                name: "IX_Areas_NombreArea",
                table: "Areas",
                column: "NombreArea",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Usuarios_Areas_IdArea",
                table: "Usuarios",
                column: "IdArea",
                principalTable: "Areas",
                principalColumn: "IdArea",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Usuarios_Areas_IdArea",
                table: "Usuarios");

            migrationBuilder.DropTable(
                name: "Areas");

            migrationBuilder.DropIndex(
                name: "IX_Usuarios_IdArea",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "IdArea",
                table: "Usuarios");
        }
    }
}
