using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinTrack.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ExpandContasComCategoria : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ativa",
                table: "Contas",
                type: "INTEGER",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<Guid>(
                name: "categoria_id",
                table: "Contas",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "classificador",
                table: "Contas",
                type: "TEXT",
                maxLength: 3,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Contas_categoria_id",
                table: "Contas",
                column: "categoria_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Contas_categorias_categoria_id",
                table: "Contas",
                column: "categoria_id",
                principalTable: "categorias",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contas_categorias_categoria_id",
                table: "Contas");

            migrationBuilder.DropIndex(
                name: "IX_Contas_categoria_id",
                table: "Contas");

            migrationBuilder.DropColumn(
                name: "ativa",
                table: "Contas");

            migrationBuilder.DropColumn(
                name: "categoria_id",
                table: "Contas");

            migrationBuilder.DropColumn(
                name: "classificador",
                table: "Contas");
        }
    }
}
