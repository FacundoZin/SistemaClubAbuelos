using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace APIClub.Migrations
{
    /// <inheritdoc />
    public partial class siembraInicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MontoCuota",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MontoCuotaFija = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FechaActualizacion = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MontoCuota", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Salones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false),
                    Direccion = table.Column<string>(type: "TEXT", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Salones", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Socios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nombre = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Apellido = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Dni = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                    Telefono = table.Column<string>(type: "TEXT", nullable: true),
                    Direcccion = table.Column<string>(type: "TEXT", nullable: true),
                    Lote = table.Column<string>(type: "TEXT", nullable: true),
                    Localidad = table.Column<string>(type: "TEXT", nullable: true),
                    FechaAsociacion = table.Column<DateOnly>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Socios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cuotas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FechaPago = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Monto = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FormaDePago = table.Column<int>(type: "INTEGER", nullable: false),
                    Anio = table.Column<int>(type: "INTEGER", nullable: false),
                    Semestre = table.Column<int>(type: "INTEGER", nullable: false),
                    SocioId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cuotas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cuotas_Socios_SocioId",
                        column: x => x.SocioId,
                        principalTable: "Socios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReservasSalones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FechaAlquiler = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Importe = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalPagado = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SocioId = table.Column<int>(type: "INTEGER", nullable: false),
                    SalonId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReservasSalones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReservasSalones_Salones_SalonId",
                        column: x => x.SalonId,
                        principalTable: "Salones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReservasSalones_Socios_SocioId",
                        column: x => x.SocioId,
                        principalTable: "Socios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "MontoCuota",
                columns: new[] { "Id", "FechaActualizacion", "MontoCuotaFija" },
                values: new object[] { 1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2500.00m });

            migrationBuilder.InsertData(
                table: "Salones",
                columns: new[] { "Id", "Direccion", "Name" },
                values: new object[,]
                {
                    { 1, "Calle Falsa 123", "Salón Central" },
                    { 2, "Av. Siempre Viva 742", "Salón Norte" }
                });

            migrationBuilder.InsertData(
                table: "Socios",
                columns: new[] { "Id", "Apellido", "Direcccion", "Dni", "FechaAsociacion", "Localidad", "Lote", "Nombre", "Telefono" },
                values: new object[,]
                {
                    { 1, "Pérez", "Mitre 100", "12345678", new DateOnly(2020, 5, 10), "Rosario", null, "Juan", "341-1234567" },
                    { 2, "Gómez", "San Martín 200", "87654321", new DateOnly(2021, 3, 15), "Córdoba", null, "María", "341-7654321" }
                });

            migrationBuilder.InsertData(
                table: "Cuotas",
                columns: new[] { "Id", "Anio", "FechaPago", "FormaDePago", "Monto", "Semestre", "SocioId" },
                values: new object[,]
                {
                    { 1, 2024, new DateTime(2024, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 2500.00m, 1, 1 },
                    { 2, 2024, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 2500.00m, 2, 1 }
                });

            migrationBuilder.InsertData(
                table: "ReservasSalones",
                columns: new[] { "Id", "FechaAlquiler", "Importe", "SalonId", "SocioId", "TotalPagado" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 5, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 5000.00m, 1, 1, 0.00m },
                    { 2, new DateTime(2025, 6, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 7000.00m, 2, 2, 7000.00m }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cuotas_SocioId",
                table: "Cuotas",
                column: "SocioId");

            migrationBuilder.CreateIndex(
                name: "IX_ReservasSalones_SalonId",
                table: "ReservasSalones",
                column: "SalonId");

            migrationBuilder.CreateIndex(
                name: "IX_ReservasSalones_SocioId",
                table: "ReservasSalones",
                column: "SocioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cuotas");

            migrationBuilder.DropTable(
                name: "MontoCuota");

            migrationBuilder.DropTable(
                name: "ReservasSalones");

            migrationBuilder.DropTable(
                name: "Salones");

            migrationBuilder.DropTable(
                name: "Socios");
        }
    }
}
