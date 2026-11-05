using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinTrack.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddCategorias : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "categorias",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "TEXT", nullable: false),
                    nome = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    nome_normalizado = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    natureza = table.Column<string>(type: "TEXT", maxLength: 10, nullable: false),
                    ativa = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_categorias", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_categorias_nome_normalizado",
                table: "categorias",
                column: "nome_normalizado",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "categorias");
        }
    }
}
