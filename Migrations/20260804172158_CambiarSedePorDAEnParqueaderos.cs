using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HelpDesk.API.Migrations
{
    /// <inheritdoc />
    public partial class CambiarSedePorDAEnParqueaderos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Parqueaderos_Sedes_IdSede",
                table: "Parqueaderos");

            migrationBuilder.DropIndex(
                name: "IX_Parqueaderos_IdSede",
                table: "Parqueaderos");

            migrationBuilder.DropColumn(
                name: "IdSede",
                table: "Parqueaderos");

            migrationBuilder.AddColumn<string>(
                name: "DA",
                table: "Parqueaderos",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            // Actualizar registros existentes con valores únicos
            migrationBuilder.Sql(@"
                UPDATE ""Parqueaderos"" 
                SET ""DA"" = 'DA-' || LPAD(""IdParqueadero""::text, 3, '0')
                WHERE ""DA"" IS NULL;
            ");

            migrationBuilder.AlterColumn<string>(
                name: "DA",
                table: "Parqueaderos",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Parqueaderos_DA",
                table: "Parqueaderos",
                column: "DA",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Parqueaderos_DA",
                table: "Parqueaderos");

            migrationBuilder.DropColumn(
                name: "DA",
                table: "Parqueaderos");

            migrationBuilder.AddColumn<int>(
                name: "IdSede",
                table: "Parqueaderos",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Parqueaderos_IdSede",
                table: "Parqueaderos",
                column: "IdSede");

            migrationBuilder.AddForeignKey(
                name: "FK_Parqueaderos_Sedes_IdSede",
                table: "Parqueaderos",
                column: "IdSede",
                principalTable: "Sedes",
                principalColumn: "IdSede",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
