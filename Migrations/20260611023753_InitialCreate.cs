using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace HelpDesk.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Canales",
                columns: table => new
                {
                    IdCanal = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "text", nullable: false),
                    FechaSolicitud = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Canales", x => x.IdCanal);
                });

            migrationBuilder.CreateTable(
                name: "CategoriasActivo",
                columns: table => new
                {
                    IdCategoria = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "text", nullable: false),
                    Estado = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoriasActivo", x => x.IdCategoria);
                });

            migrationBuilder.CreateTable(
                name: "OrdenesCompra",
                columns: table => new
                {
                    IdOrden = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NumeroOC = table.Column<string>(type: "text", nullable: false),
                    Proveedor = table.Column<string>(type: "text", nullable: false),
                    Total = table.Column<decimal>(type: "numeric", nullable: false),
                    Observaciones = table.Column<string>(type: "text", nullable: false),
                    FechaCompra = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrdenesCompra", x => x.IdOrden);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    IdRol = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "text", nullable: false),
                    Tipo = table.Column<string>(type: "text", nullable: false),
                    Estado = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.IdRol);
                });

            migrationBuilder.CreateTable(
                name: "Sedes",
                columns: table => new
                {
                    IdSede = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "text", nullable: false),
                    Direccion = table.Column<string>(type: "text", nullable: false),
                    Ciudad = table.Column<string>(type: "text", nullable: false),
                    Estado = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sedes", x => x.IdSede);
                });

            migrationBuilder.CreateTable(
                name: "Activos",
                columns: table => new
                {
                    IdActivo = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdCategoria = table.Column<int>(type: "integer", nullable: false),
                    IdOrden = table.Column<int>(type: "integer", nullable: false),
                    CodigoActivo = table.Column<string>(type: "text", nullable: false),
                    Serial = table.Column<string>(type: "text", nullable: false),
                    Marca = table.Column<string>(type: "text", nullable: false),
                    Modelo = table.Column<string>(type: "text", nullable: false),
                    Referencia = table.Column<string>(type: "text", nullable: false),
                    Disponible = table.Column<bool>(type: "boolean", nullable: false),
                    FechaAdquisicion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaBaja = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Observaciones = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activos", x => x.IdActivo);
                    table.ForeignKey(
                        name: "FK_Activos_CategoriasActivo_IdCategoria",
                        column: x => x.IdCategoria,
                        principalTable: "CategoriasActivo",
                        principalColumn: "IdCategoria",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Activos_OrdenesCompra_IdOrden",
                        column: x => x.IdOrden,
                        principalTable: "OrdenesCompra",
                        principalColumn: "IdOrden",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Parqueaderos",
                columns: table => new
                {
                    IdParqueadero = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdSede = table.Column<int>(type: "integer", nullable: false),
                    Nombre = table.Column<string>(type: "text", nullable: false),
                    Ubicacion = table.Column<string>(type: "text", nullable: false),
                    Estado = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parqueaderos", x => x.IdParqueadero);
                    table.ForeignKey(
                        name: "FK_Parqueaderos_Sedes_IdSede",
                        column: x => x.IdSede,
                        principalTable: "Sedes",
                        principalColumn: "IdSede",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    IdUsuario = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdRol = table.Column<int>(type: "integer", nullable: false),
                    IdSede = table.Column<int>(type: "integer", nullable: false),
                    Nombre = table.Column<string>(type: "text", nullable: false),
                    Correo = table.Column<string>(type: "text", nullable: false),
                    Telefono = table.Column<string>(type: "text", nullable: false),
                    Cargo = table.Column<string>(type: "text", nullable: false),
                    Contraseña = table.Column<string>(type: "text", nullable: false),
                    Estado = table.Column<bool>(type: "boolean", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.IdUsuario);
                    table.ForeignKey(
                        name: "FK_Usuarios_Roles_IdRol",
                        column: x => x.IdRol,
                        principalTable: "Roles",
                        principalColumn: "IdRol",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Usuarios_Sedes_IdSede",
                        column: x => x.IdSede,
                        principalTable: "Sedes",
                        principalColumn: "IdSede",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AsignacionesUsuario",
                columns: table => new
                {
                    IdAsignacion = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdActivo = table.Column<int>(type: "integer", nullable: false),
                    IdUsuarioDestino = table.Column<int>(type: "integer", nullable: false),
                    IdParqueadero = table.Column<int>(type: "integer", nullable: true),
                    FechaAsignacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Activo = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AsignacionesUsuario", x => x.IdAsignacion);
                    table.ForeignKey(
                        name: "FK_AsignacionesUsuario_Activos_IdActivo",
                        column: x => x.IdActivo,
                        principalTable: "Activos",
                        principalColumn: "IdActivo",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AsignacionesUsuario_Parqueaderos_IdParqueadero",
                        column: x => x.IdParqueadero,
                        principalTable: "Parqueaderos",
                        principalColumn: "IdParqueadero",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_AsignacionesUsuario_Usuarios_IdUsuarioDestino",
                        column: x => x.IdUsuarioDestino,
                        principalTable: "Usuarios",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Salidas",
                columns: table => new
                {
                    IdSalida = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdCanal = table.Column<int>(type: "integer", nullable: false),
                    CodigoUnico = table.Column<string>(type: "text", nullable: false),
                    NumeroTicket = table.Column<string>(type: "text", nullable: true),
                    IdUsuarioDestino = table.Column<int>(type: "integer", nullable: true),
                    IdParqueaderoDestino = table.Column<int>(type: "integer", nullable: true),
                    IdUsuarioEntrega = table.Column<int>(type: "integer", nullable: false),
                    FechaSalida = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RegistroSalida = table.Column<string>(type: "text", nullable: false),
                    Observaciones = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Salidas", x => x.IdSalida);
                    table.CheckConstraint("CK_Salida_Destino", "\"IdUsuarioDestino\" IS NOT NULL OR \"IdParqueaderoDestino\" IS NOT NULL");
                    table.ForeignKey(
                        name: "FK_Salidas_Canales_IdCanal",
                        column: x => x.IdCanal,
                        principalTable: "Canales",
                        principalColumn: "IdCanal",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Salidas_Parqueaderos_IdParqueaderoDestino",
                        column: x => x.IdParqueaderoDestino,
                        principalTable: "Parqueaderos",
                        principalColumn: "IdParqueadero",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Salidas_Usuarios_IdUsuarioDestino",
                        column: x => x.IdUsuarioDestino,
                        principalTable: "Usuarios",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Salidas_Usuarios_IdUsuarioEntrega",
                        column: x => x.IdUsuarioEntrega,
                        principalTable: "Usuarios",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DetallesSalida",
                columns: table => new
                {
                    IdDetalleSalida = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdSalida = table.Column<int>(type: "integer", nullable: false),
                    IdActivo = table.Column<int>(type: "integer", nullable: false),
                    Cantidad = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetallesSalida", x => x.IdDetalleSalida);
                    table.ForeignKey(
                        name: "FK_DetallesSalida_Activos_IdActivo",
                        column: x => x.IdActivo,
                        principalTable: "Activos",
                        principalColumn: "IdActivo",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DetallesSalida_Salidas_IdSalida",
                        column: x => x.IdSalida,
                        principalTable: "Salidas",
                        principalColumn: "IdSalida",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HistorialActivos",
                columns: table => new
                {
                    IdHistorial = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdActivo = table.Column<int>(type: "integer", nullable: false),
                    IdSalida = table.Column<int>(type: "integer", nullable: false),
                    TipoMovimiento = table.Column<string>(type: "text", nullable: false),
                    FechaMovimiento = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IdUsuarioEntrega = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistorialActivos", x => x.IdHistorial);
                    table.ForeignKey(
                        name: "FK_HistorialActivos_Activos_IdActivo",
                        column: x => x.IdActivo,
                        principalTable: "Activos",
                        principalColumn: "IdActivo",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HistorialActivos_Salidas_IdSalida",
                        column: x => x.IdSalida,
                        principalTable: "Salidas",
                        principalColumn: "IdSalida",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HistorialActivos_Usuarios_IdUsuarioEntrega",
                        column: x => x.IdUsuarioEntrega,
                        principalTable: "Usuarios",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Activos_CodigoActivo",
                table: "Activos",
                column: "CodigoActivo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Activos_IdCategoria",
                table: "Activos",
                column: "IdCategoria");

            migrationBuilder.CreateIndex(
                name: "IX_Activos_IdOrden",
                table: "Activos",
                column: "IdOrden");

            migrationBuilder.CreateIndex(
                name: "IX_Activos_Serial",
                table: "Activos",
                column: "Serial",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Asignacion_ActivoUnico",
                table: "AsignacionesUsuario",
                columns: new[] { "IdActivo", "Activo" },
                unique: true,
                filter: "\"Activo\" = true");

            migrationBuilder.CreateIndex(
                name: "IX_AsignacionesUsuario_IdParqueadero",
                table: "AsignacionesUsuario",
                column: "IdParqueadero");

            migrationBuilder.CreateIndex(
                name: "IX_AsignacionesUsuario_IdUsuarioDestino",
                table: "AsignacionesUsuario",
                column: "IdUsuarioDestino");

            migrationBuilder.CreateIndex(
                name: "IX_Canales_Nombre",
                table: "Canales",
                column: "Nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CategoriasActivo_Nombre",
                table: "CategoriasActivo",
                column: "Nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DetallesSalida_IdActivo",
                table: "DetallesSalida",
                column: "IdActivo");

            migrationBuilder.CreateIndex(
                name: "IX_DetallesSalida_IdSalida",
                table: "DetallesSalida",
                column: "IdSalida");

            migrationBuilder.CreateIndex(
                name: "IX_HistorialActivos_IdActivo",
                table: "HistorialActivos",
                column: "IdActivo");

            migrationBuilder.CreateIndex(
                name: "IX_HistorialActivos_IdSalida",
                table: "HistorialActivos",
                column: "IdSalida");

            migrationBuilder.CreateIndex(
                name: "IX_HistorialActivos_IdUsuarioEntrega",
                table: "HistorialActivos",
                column: "IdUsuarioEntrega");

            migrationBuilder.CreateIndex(
                name: "IX_OrdenesCompra_NumeroOC",
                table: "OrdenesCompra",
                column: "NumeroOC",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Parqueaderos_IdSede",
                table: "Parqueaderos",
                column: "IdSede");

            migrationBuilder.CreateIndex(
                name: "IX_Salidas_CodigoUnico",
                table: "Salidas",
                column: "CodigoUnico",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Salidas_IdCanal",
                table: "Salidas",
                column: "IdCanal");

            migrationBuilder.CreateIndex(
                name: "IX_Salidas_IdParqueaderoDestino",
                table: "Salidas",
                column: "IdParqueaderoDestino");

            migrationBuilder.CreateIndex(
                name: "IX_Salidas_IdUsuarioDestino",
                table: "Salidas",
                column: "IdUsuarioDestino");

            migrationBuilder.CreateIndex(
                name: "IX_Salidas_IdUsuarioEntrega",
                table: "Salidas",
                column: "IdUsuarioEntrega");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_Correo",
                table: "Usuarios",
                column: "Correo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_IdRol",
                table: "Usuarios",
                column: "IdRol");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_IdSede",
                table: "Usuarios",
                column: "IdSede");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AsignacionesUsuario");

            migrationBuilder.DropTable(
                name: "DetallesSalida");

            migrationBuilder.DropTable(
                name: "HistorialActivos");

            migrationBuilder.DropTable(
                name: "Activos");

            migrationBuilder.DropTable(
                name: "Salidas");

            migrationBuilder.DropTable(
                name: "CategoriasActivo");

            migrationBuilder.DropTable(
                name: "OrdenesCompra");

            migrationBuilder.DropTable(
                name: "Canales");

            migrationBuilder.DropTable(
                name: "Parqueaderos");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Sedes");
        }
    }
}
