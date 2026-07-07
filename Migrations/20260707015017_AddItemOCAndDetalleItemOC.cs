using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace HelpDesk.API.Migrations
{
    /// <inheritdoc />
    public partial class AddItemOCAndDetalleItemOC : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Referencia",
                table: "Activos",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<int>(
                name: "IdDetalleItemOC",
                table: "Activos",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IdItemOC",
                table: "Activos",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ItemsOC",
                columns: table => new
                {
                    IdItemOC = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdOrden = table.Column<int>(type: "integer", nullable: false),
                    IdCategoria = table.Column<int>(type: "integer", nullable: false),
                    NombreProducto = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Marca = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Modelo = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Referencia = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Observaciones = table.Column<string>(type: "text", nullable: true),
                    CantidadEsperada = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemsOC", x => x.IdItemOC);
                    table.ForeignKey(
                        name: "FK_ItemsOC_CategoriasActivo_IdCategoria",
                        column: x => x.IdCategoria,
                        principalTable: "CategoriasActivo",
                        principalColumn: "IdCategoria",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ItemsOC_OrdenesCompra_IdOrden",
                        column: x => x.IdOrden,
                        principalTable: "OrdenesCompra",
                        principalColumn: "IdOrden",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DetallesItemOC",
                columns: table => new
                {
                    IdDetalleItemOC = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdItemOC = table.Column<int>(type: "integer", nullable: false),
                    Serial = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Procesado = table.Column<bool>(type: "boolean", nullable: false),
                    IdActivo = table.Column<int>(type: "integer", nullable: true),
                    Observaciones = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetallesItemOC", x => x.IdDetalleItemOC);
                    table.ForeignKey(
                        name: "FK_DetallesItemOC_Activos_IdActivo",
                        column: x => x.IdActivo,
                        principalTable: "Activos",
                        principalColumn: "IdActivo",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_DetallesItemOC_ItemsOC_IdItemOC",
                        column: x => x.IdItemOC,
                        principalTable: "ItemsOC",
                        principalColumn: "IdItemOC",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Activos_IdDetalleItemOC",
                table: "Activos",
                column: "IdDetalleItemOC");

            migrationBuilder.CreateIndex(
                name: "IX_Activos_IdItemOC",
                table: "Activos",
                column: "IdItemOC");

            migrationBuilder.CreateIndex(
                name: "IX_DetallesItemOC_IdActivo",
                table: "DetallesItemOC",
                column: "IdActivo");

            migrationBuilder.CreateIndex(
                name: "IX_DetallesItemOC_IdItemOC",
                table: "DetallesItemOC",
                column: "IdItemOC");

            migrationBuilder.CreateIndex(
                name: "IX_ItemsOC_IdCategoria",
                table: "ItemsOC",
                column: "IdCategoria");

            migrationBuilder.CreateIndex(
                name: "IX_ItemsOC_IdOrden",
                table: "ItemsOC",
                column: "IdOrden");

            migrationBuilder.AddForeignKey(
                name: "FK_Activos_DetallesItemOC_IdDetalleItemOC",
                table: "Activos",
                column: "IdDetalleItemOC",
                principalTable: "DetallesItemOC",
                principalColumn: "IdDetalleItemOC",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Activos_ItemsOC_IdItemOC",
                table: "Activos",
                column: "IdItemOC",
                principalTable: "ItemsOC",
                principalColumn: "IdItemOC",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activos_DetallesItemOC_IdDetalleItemOC",
                table: "Activos");

            migrationBuilder.DropForeignKey(
                name: "FK_Activos_ItemsOC_IdItemOC",
                table: "Activos");

            migrationBuilder.DropTable(
                name: "DetallesItemOC");

            migrationBuilder.DropTable(
                name: "ItemsOC");

            migrationBuilder.DropIndex(
                name: "IX_Activos_IdDetalleItemOC",
                table: "Activos");

            migrationBuilder.DropIndex(
                name: "IX_Activos_IdItemOC",
                table: "Activos");

            migrationBuilder.DropColumn(
                name: "IdDetalleItemOC",
                table: "Activos");

            migrationBuilder.DropColumn(
                name: "IdItemOC",
                table: "Activos");

            migrationBuilder.AlterColumn<string>(
                name: "Referencia",
                table: "Activos",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }
    }
}
