using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class FixHistoricoPedido : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_HistoricoStatusPedido_PedidoId",
                table: "HistoricoStatusPedido");

            migrationBuilder.CreateIndex(
                name: "IX_HistoricoStatusPedido_PedidoId",
                table: "HistoricoStatusPedido",
                column: "PedidoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_HistoricoStatusPedido_PedidoId",
                table: "HistoricoStatusPedido");

            migrationBuilder.CreateIndex(
                name: "IX_HistoricoStatusPedido_PedidoId",
                table: "HistoricoStatusPedido",
                column: "PedidoId",
                unique: true);
        }
    }
}
