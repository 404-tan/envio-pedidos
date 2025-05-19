using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class MapeandoUsuario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HistoricoStatusPedido_AspNetUsers_AdministradorId",
                table: "HistoricoStatusPedido");

            migrationBuilder.DropForeignKey(
                name: "FK_HistoricoStatusPedido_Pedido_PedidoId",
                table: "HistoricoStatusPedido");

            migrationBuilder.DropForeignKey(
                name: "FK_ItemPedido_Pedido_PedidoId",
                table: "ItemPedido");

            migrationBuilder.DropForeignKey(
                name: "FK_ItemPedido_Produto_ProdutoId",
                table: "ItemPedido");

            migrationBuilder.DropForeignKey(
                name: "FK_Pedido_AspNetUsers_ClienteId",
                table: "Pedido");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Produto",
                table: "Produto");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Pedido",
                table: "Pedido");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HistoricoStatusPedido",
                table: "HistoricoStatusPedido");

            migrationBuilder.DropIndex(
                name: "IX_HistoricoStatusPedido_AdministradorId",
                table: "HistoricoStatusPedido");

            migrationBuilder.DropColumn(
                name: "AdministradorId",
                table: "HistoricoStatusPedido");

            migrationBuilder.RenameTable(
                name: "Produto",
                newName: "Produtos");

            migrationBuilder.RenameTable(
                name: "Pedido",
                newName: "Pedidos");

            migrationBuilder.RenameTable(
                name: "HistoricoStatusPedido",
                newName: "HistoricoStatusPedidos");

            migrationBuilder.RenameIndex(
                name: "IX_Pedido_ClienteId",
                table: "Pedidos",
                newName: "IX_Pedidos_ClienteId");

            migrationBuilder.RenameIndex(
                name: "IX_HistoricoStatusPedido_PedidoId",
                table: "HistoricoStatusPedidos",
                newName: "IX_HistoricoStatusPedidos_PedidoId");

            migrationBuilder.AddColumn<DateTime>(
                name: "DataAtualizacao",
                table: "Pedidos",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProcessadoPorAdministradorId",
                table: "Pedidos",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UsuarioId",
                table: "HistoricoStatusPedidos",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Produtos",
                table: "Produtos",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Pedidos",
                table: "Pedidos",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HistoricoStatusPedidos",
                table: "HistoricoStatusPedidos",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Pedidos_IdCliente",
                table: "Pedidos",
                column: "IdCliente");

            migrationBuilder.CreateIndex(
                name: "IX_Pedidos_ProcessadoPorAdministradorId",
                table: "Pedidos",
                column: "ProcessadoPorAdministradorId");

            migrationBuilder.CreateIndex(
                name: "IX_HistoricoStatusPedidos_UsuarioId",
                table: "HistoricoStatusPedidos",
                column: "UsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_HistoricoStatusPedidos_AspNetUsers_UsuarioId",
                table: "HistoricoStatusPedidos",
                column: "UsuarioId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_HistoricoStatusPedidos_Pedidos_PedidoId",
                table: "HistoricoStatusPedidos",
                column: "PedidoId",
                principalTable: "Pedidos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ItemPedido_Pedidos_PedidoId",
                table: "ItemPedido",
                column: "PedidoId",
                principalTable: "Pedidos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ItemPedido_Produtos_ProdutoId",
                table: "ItemPedido",
                column: "ProdutoId",
                principalTable: "Produtos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Pedidos_AspNetUsers_ClienteId",
                table: "Pedidos",
                column: "ClienteId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Pedidos_AspNetUsers_IdCliente",
                table: "Pedidos",
                column: "IdCliente",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Pedidos_AspNetUsers_ProcessadoPorAdministradorId",
                table: "Pedidos",
                column: "ProcessadoPorAdministradorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HistoricoStatusPedidos_AspNetUsers_UsuarioId",
                table: "HistoricoStatusPedidos");

            migrationBuilder.DropForeignKey(
                name: "FK_HistoricoStatusPedidos_Pedidos_PedidoId",
                table: "HistoricoStatusPedidos");

            migrationBuilder.DropForeignKey(
                name: "FK_ItemPedido_Pedidos_PedidoId",
                table: "ItemPedido");

            migrationBuilder.DropForeignKey(
                name: "FK_ItemPedido_Produtos_ProdutoId",
                table: "ItemPedido");

            migrationBuilder.DropForeignKey(
                name: "FK_Pedidos_AspNetUsers_ClienteId",
                table: "Pedidos");

            migrationBuilder.DropForeignKey(
                name: "FK_Pedidos_AspNetUsers_IdCliente",
                table: "Pedidos");

            migrationBuilder.DropForeignKey(
                name: "FK_Pedidos_AspNetUsers_ProcessadoPorAdministradorId",
                table: "Pedidos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Produtos",
                table: "Produtos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Pedidos",
                table: "Pedidos");

            migrationBuilder.DropIndex(
                name: "IX_Pedidos_IdCliente",
                table: "Pedidos");

            migrationBuilder.DropIndex(
                name: "IX_Pedidos_ProcessadoPorAdministradorId",
                table: "Pedidos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HistoricoStatusPedidos",
                table: "HistoricoStatusPedidos");

            migrationBuilder.DropIndex(
                name: "IX_HistoricoStatusPedidos_UsuarioId",
                table: "HistoricoStatusPedidos");

            migrationBuilder.DropColumn(
                name: "DataAtualizacao",
                table: "Pedidos");

            migrationBuilder.DropColumn(
                name: "ProcessadoPorAdministradorId",
                table: "Pedidos");

            migrationBuilder.DropColumn(
                name: "UsuarioId",
                table: "HistoricoStatusPedidos");

            migrationBuilder.RenameTable(
                name: "Produtos",
                newName: "Produto");

            migrationBuilder.RenameTable(
                name: "Pedidos",
                newName: "Pedido");

            migrationBuilder.RenameTable(
                name: "HistoricoStatusPedidos",
                newName: "HistoricoStatusPedido");

            migrationBuilder.RenameIndex(
                name: "IX_Pedidos_ClienteId",
                table: "Pedido",
                newName: "IX_Pedido_ClienteId");

            migrationBuilder.RenameIndex(
                name: "IX_HistoricoStatusPedidos_PedidoId",
                table: "HistoricoStatusPedido",
                newName: "IX_HistoricoStatusPedido_PedidoId");

            migrationBuilder.AddColumn<Guid>(
                name: "AdministradorId",
                table: "HistoricoStatusPedido",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Produto",
                table: "Produto",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Pedido",
                table: "Pedido",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HistoricoStatusPedido",
                table: "HistoricoStatusPedido",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_HistoricoStatusPedido_AdministradorId",
                table: "HistoricoStatusPedido",
                column: "AdministradorId");

            migrationBuilder.AddForeignKey(
                name: "FK_HistoricoStatusPedido_AspNetUsers_AdministradorId",
                table: "HistoricoStatusPedido",
                column: "AdministradorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_HistoricoStatusPedido_Pedido_PedidoId",
                table: "HistoricoStatusPedido",
                column: "PedidoId",
                principalTable: "Pedido",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ItemPedido_Pedido_PedidoId",
                table: "ItemPedido",
                column: "PedidoId",
                principalTable: "Pedido",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ItemPedido_Produto_ProdutoId",
                table: "ItemPedido",
                column: "ProdutoId",
                principalTable: "Produto",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Pedido_AspNetUsers_ClienteId",
                table: "Pedido",
                column: "ClienteId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
