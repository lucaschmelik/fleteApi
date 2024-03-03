using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace fleteApi.Migrations
{
    public partial class Inicial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Localidades",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Localidad_pk", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Viajes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HorarioDesde = table.Column<int>(type: "int", nullable: true),
                    HorarioHasta = table.Column<int>(type: "int", nullable: true),
                    Barrio = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    Direccion = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    Recibe = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Envia = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Telefono = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    Completado = table.Column<DateTime>(type: "datetime", nullable: true),
                    FechaEntrega = table.Column<DateTime>(type: "datetime", nullable: true),
                    Orden = table.Column<int>(type: "int", nullable: true),
                    Observaciones = table.Column<string>(type: "varchar(150)", unicode: false, maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Viaje_pk", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Localidades");

            migrationBuilder.DropTable(
                name: "Viajes");
        }
    }
}
