using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinTrack.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddCargasIniciais : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "cargas_iniciais",
                columns: table => new
                {
                    identificador = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    data_aplicacao_utc = table.Column<DateTimeOffset>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cargas_iniciais", x => x.identificador);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cargas_iniciais");
        }
    }
}
