using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinTrack.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddLancamentos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "lancamentos",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "TEXT", nullable: false),
                    conta_id = table.Column<Guid>(type: "TEXT", nullable: false),
                    valor = table.Column<decimal>(type: "TEXT", precision: 18, scale: 2, nullable: false),
                    data_realizacao = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    descricao = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    situacao = table.Column<string>(type: "TEXT", maxLength: 10, nullable: false),
                    data_cancelamento = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    motivo_cancelamento = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lancamentos", x => x.id);
                    table.ForeignKey(
                        name: "FK_lancamentos_Contas_conta_id",
                        column: x => x.conta_id,
                        principalTable: "Contas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_lancamentos_conta_id",
                table: "lancamentos",
                column: "conta_id");

            migrationBuilder.CreateIndex(
                name: "IX_lancamentos_data_realizacao_id",
                table: "lancamentos",
                columns: new[] { "data_realizacao", "id" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "lancamentos");
        }
    }
}
